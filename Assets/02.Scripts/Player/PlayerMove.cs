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
    
    public PlayerStates PlayerState = PlayerStates.Idle;

    [SerializeField]
    private float _currentSpeed;
    private const float GRAVITY = -9.81f;   // 중력가속도
    private float _yVelocity = 0f;  // 중력 변수
    
    // 스테미나
    [SerializeField]
    private float _currentStamina;
    
    // 슬라이드
    private float _slideTimer = 0f;
    private bool _isSliding = false;

    private bool _isJumping = false;

    private float _h = 0f;
    private float _v = 0f;

    private CharacterController _characterController;

    private bool _isClimbing = false;
    private Vector3 _climbNormal;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _currentSpeed = _playerData.MoveSpeed;
        _currentStamina = _playerData.MaxStamina;
    }

    private void Update() 
    {
        Vector3 moveDirection = GetMoveDirection();
        Jump();
        ApplyGravity();
        Run();
        Slide();
        Climb(moveDirection);
        
        // 스태미나 회복 - 실제 움직임이 없을 때만 회복
        if(moveDirection.magnitude < 0.1f)  // 움직임이 거의 없을 때
        {
            _currentStamina += _playerData.AddStamina * Time.deltaTime;
            UIManager.Instance.UpdateStamina(_currentStamina, _playerData.MaxStamina);
        }

        moveDirection.y = _yVelocity;

        _currentStamina = Mathf.Clamp(_currentStamina, 0f, _playerData.MaxStamina);
        _characterController.Move(moveDirection * _currentSpeed * Time.deltaTime);
    }
    
    private Vector3 GetMoveDirection()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(_h, 0, _v).normalized;
        // 메인 카메라를 기준으로 방향을 변환한다.
        return Camera.main.transform.TransformDirection(dir);
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
        if(_currentStamina > _playerData.MinRunStamina)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                PlayerState = PlayerStates.Run;
                _currentSpeed = _playerData.RunSpeed;
                _currentStamina -= _playerData.SubStamina * Time.deltaTime;
                UIManager.Instance.UpdateStamina(_currentStamina, _playerData.MaxStamina);
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
                if(_currentStamina > _playerData.SlidingStamina)
                {
                    _currentSpeed = _playerData.SlideSpeed;
                    _currentStamina -= _playerData.SlidingStamina;
                    UIManager.Instance.UpdateStamina(_currentStamina, _playerData.MaxStamina);
                    _isSliding = true;
                    PlayerState = PlayerStates.Slide;
                }
            }
        }
    }

    private void Climb(Vector3 moveDirection)
    {
        if (_characterController.collisionFlags == CollisionFlags.Sides)
        {
            PlayerState = PlayerStates.Climb;
            _isClimbing = true;
            _yVelocity = 0f;

            _currentStamina -= _playerData.SubStamina * 2 * Time.deltaTime;
            if(_currentStamina <= 5)
            {
                PlayerState = PlayerStates.Idle;
            }
            UIManager.Instance.UpdateStamina(_currentStamina, _playerData.MaxStamina);

            if (Input.GetKey(KeyCode.W))
            {
                _yVelocity = _playerData.ClimbSpeed;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _yVelocity = -_playerData.ClimbSpeed;
            }
            else
            {
                _yVelocity = 0f;
            }
        }
        else if (_isClimbing)
        {
            _isClimbing = false;
            PlayerState = PlayerStates.Idle;
        }
    }
}
