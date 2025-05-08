using UnityEngine;

public enum CameraMode
    {
        FirstPerson,
        ThirdPerson,
        TopDown,
    }

public class CameraFollow : MonoBehaviour
{
    public Transform FPSTarget;
    public Transform TPSTarget;
    
    [SerializeField]
    private Transform _target;
    [SerializeField] private Vector3 _fpsOffset = new Vector3(0, 1.5f, -1.5f);
    [Header("3인칭 카메라 설정")]
    [SerializeField] private float _tpsDistance = 5f; // 카메라와 플레이어 사이의 거리
    [SerializeField] private float _tpsHeight = 2f; // 카메라의 높이
    [SerializeField] private float _smoothSpeed = 10f; // 카메라 이동 부드러움
    
    
    [Header("탑다운 카메라 설정")]
    [SerializeField] private Vector3 _topDownOffset = new Vector3(0, 10, -5);
    
    private Vector3 _currentVelocity;
    private CameraMode _currentCameraMode;
    private GameObject _player;
    public CameraMode CurrentCameraMode => _currentCameraMode;

    private Vector3 _lastTargetPosition;
    private Vector3 _lookAheadPos;
    private bool _isShaking = false;
    private float _shakeIntensity = 0.2f;
    private float _shakeDuration = 0.3f;
    private float _shakeTimer = 0f;
    private Vector3 _shakeOffset;

    private void Start()
    {
        _target = FPSTarget;
        _player = GameObject.FindGameObjectWithTag("Player");
        _currentCameraMode = CameraMode.FirstPerson;
        _lastTargetPosition = _target.position;
    }
    
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _currentCameraMode = CameraMode.FirstPerson;
            _target = FPSTarget;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _currentCameraMode = CameraMode.ThirdPerson;
            _target = TPSTarget;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _currentCameraMode = CameraMode.TopDown;
            _target = _player.transform;
        }
        
        Vector3 targetPosition = transform.position;
        
        if (_currentCameraMode == CameraMode.FirstPerson)
        {
            // 1인칭 카메라
            targetPosition = _target.position;
        }
        else if (_currentCameraMode == CameraMode.ThirdPerson)
        {
            // 3인칭 카메라
            // 카메라 위치 계산
            Vector3 desiredPosition = _target.position - _target.forward * _tpsDistance + Vector3.up * _tpsHeight;
            
            // 부드러운 카메라 이동
            targetPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _currentVelocity, 1f / _smoothSpeed);
        }
        else if (_currentCameraMode == CameraMode.TopDown)
        {
            // 탑다운 카메라
            Vector3 desiredPosition = _target.position + _topDownOffset;
            targetPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _currentVelocity, 0.1f);
        }

        // 쉐이크 효과 적용
        if (_isShaking)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0)
            {
                _isShaking = false;
            }
            else
            {
                targetPosition += new Vector3(
                    Random.Range(-1f, 1f) * _shakeIntensity,
                    Random.Range(-1f, 1f) * _shakeIntensity,
                    0
                );
            }
        }

        transform.position = targetPosition;
    }

    public void ShakeCamera(float intensity = 0.2f, float duration = 0.3f)
    {
        _isShaking = true;
        _shakeIntensity = intensity;
        _shakeDuration = duration;
        _shakeTimer = duration;
    }
}
