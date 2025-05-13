using System;
using Unity.Mathematics;
using UnityEngine;

public enum PlayerStates
{
    Idle,
    Run,
    Jump,
    Slide,
    Climb,
}
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private PlayerSO _playerData;
    private Player _player;
    
    public PlayerStates PlayerState = PlayerStates.Idle;

    [SerializeField]
    private float _currentSpeed;
    private const float GRAVITY = -9.81f;   // 중력가속도
    private float _yVelocity = 0f;  // 중력 변수
    
    // 슬라이드
    private float _slideTimer = 0f;
    private bool _isSliding = false;

    private bool _isJumping = false;

    private float _h = 0f;
    private float _v = 0f;

    private CharacterController _characterController;
    private CameraFollow _cameraFollow;
    private Animator _animator;

    private bool _isClimbing = false;
    private Vector3 _climbNormal;
    private float _climbRayDistance = 1.5f;  // 레이캐스트 거리
    private float _climbCheckInterval = 0.1f; // 레이캐스트 체크 간격
    private float _lastClimbCheckTime = 0f;   // 마지막 레이캐스트 체크 시간

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _player = GetComponent<Player>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _currentSpeed = _playerData.MoveSpeed;
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void Update() 
    {
        Vector3 moveDirection = GetMoveDirection();

        Jump();
        ApplyGravity();
        Run();
        Slide();
        //Climb(moveDirection);
        
        // 스태미나 회복 - 실제 움직임이 없을 때만 회복
        if(moveDirection.magnitude <= 0f)  // 움직임이 거의 없을 때
        {
            _player.RecoverStamina(_player.AddStamina * Time.deltaTime);
        }

        moveDirection.y = _yVelocity;
        _characterController.Move(moveDirection * _currentSpeed * Time.deltaTime);
    }
    
    private Vector3 GetMoveDirection()
    {
        _h = Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");

        if (_cameraFollow == null) return Vector3.zero;

        Vector3 moveDirection = Vector3.zero;

        switch (_cameraFollow.CurrentCameraMode)
        {
            case CameraMode.FirstPerson:
            case CameraMode.ThirdPerson:
                // 1인칭과 3인칭 모두 카메라 기준으로 이동
                Vector3 forward = Camera.main.transform.forward;
                Vector3 right = Camera.main.transform.right;
                forward.y = 0;
                right.y = 0;
                forward.Normalize();
                right.Normalize();
                moveDirection = forward * _v + right * _h;

                _animator.SetFloat("MoveAmount", moveDirection.magnitude);

                break;

            case CameraMode.TopDown:
                // 탑다운: 월드 좌표계 기준으로 이동
                moveDirection = new Vector3(_h, 0, _v);
                _animator.SetFloat("MoveAmount", moveDirection.magnitude);
                moveDirection.Normalize();
                break;
        }

        return moveDirection;
    }
    
    private void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(_characterController.isGrounded && _isJumping == false)
            {
                PlayerState = PlayerStates.Jump;
                _yVelocity = _playerData.JumpPower;
                _isJumping = true;
            }
            else if(_isJumping)
            {
                PlayerState = PlayerStates.Jump;
                _yVelocity = _playerData.JumpPower;
                _isJumping = false;
            }
        }
        // 착지하면 상태 Idle로 변경
        if(PlayerState == PlayerStates.Jump && _characterController.isGrounded)
        {
            PlayerState = PlayerStates.Idle;
        }
    }
    
    private void ApplyGravity()
    {
        // 중력 적용
        _yVelocity += GRAVITY * Time.deltaTime;
    }

    private void Run()
    {
        if(_player.CurrentStamina > _player.MinRunStamina)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                PlayerState = PlayerStates.Run;
                _currentSpeed = _playerData.RunSpeed;
                _player.UseStamina(_player.SubStamina * Time.deltaTime);
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                PlayerState = PlayerStates.Idle;
                _currentSpeed = _playerData.MoveSpeed;
            }
        }
        else
        {
            PlayerState = PlayerStates.Idle;
            _currentSpeed = _playerData.MoveSpeed;
        }
    }

    private void Slide()
    {
        if(_isSliding)
        {
            _slideTimer += Time.deltaTime;
            if(_slideTimer >= _playerData.SlideTime)
            {
                _isSliding = false;
                _slideTimer = 0f;
                _currentSpeed = _playerData.MoveSpeed;
                PlayerState = PlayerStates.Idle;
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.E) && _characterController.isGrounded)
            {
                if(_player.CurrentStamina > _player.SlidingStamina)
                {
                    _currentSpeed = _playerData.SlideSpeed;
                    _player.UseStamina(_player.SlidingStamina);
                    _isSliding = true;
                    PlayerState = PlayerStates.Slide;
                }
            }
        }
    }

    private void Climb(Vector3 moveDirection)
    {
        // 일정 간격으로 레이캐스트 체크
        if (Time.time - _lastClimbCheckTime >= _climbCheckInterval)
        {
            _lastClimbCheckTime = Time.time;
            
            // 전방 레이캐스트
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, _climbRayDistance))
            {
                // 벽에 닿았을 때
                if (hit.collider.CompareTag("Climbable") && !_isClimbing)
                {
                    // 스태미나가 5 이하면 벽타기 불가
                    if (_player.CurrentStamina <= 5)
                    {
                        return;
                    }

                    _isClimbing = true;
                    _climbNormal = hit.normal;
                    PlayerState = PlayerStates.Climb;
                }
            }
            else if (_isClimbing)
            {
                // 벽에서 떨어졌을 때
                _isClimbing = false;
                PlayerState = PlayerStates.Idle;
            }
        }

        // 벽타기 중일 때
        if (_isClimbing)
        {
            // 스태미나 소모
            _player.UseStamina(_player.SubStamina * 2 * Time.deltaTime);

            // 스태미나가 5 이하면 벽타기 중지
            if (_player.CurrentStamina <= 5)
            {
                _isClimbing = false;
                PlayerState = PlayerStates.Idle;
                return;
            }

            _yVelocity = 0f;  // 기본적으로 수직 속도는 0

            // 이동 입력에 따른 수직 이동
            if (moveDirection.z > 0)
            {
                _yVelocity = _playerData.ClimbSpeed;
            }
            else if (moveDirection.z < 0)
            {
                _yVelocity = -_playerData.ClimbSpeed;
            }
        }
    }
}
