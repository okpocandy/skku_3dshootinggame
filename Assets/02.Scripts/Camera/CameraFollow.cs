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

    private void Start()
    {
        _target = FPSTarget;
        _player = GameObject.FindGameObjectWithTag("Player");
        _currentCameraMode = CameraMode.FirstPerson;
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
        
        if (_currentCameraMode == CameraMode.FirstPerson)
        {
            // 1인칭 카메라
            transform.position = Vector3.SmoothDamp(transform.position, _target.position, ref _currentVelocity, 0.1f);
        }
        else if (_currentCameraMode == CameraMode.ThirdPerson)
        {
            // 3인칭 카메라
            // 카메라 위치 계산
            Vector3 desiredPosition = _target.position - _target.forward * _tpsDistance + Vector3.up * _tpsHeight;
            
            // 부드러운 카메라 이동
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _currentVelocity, 1f / _smoothSpeed);
        }
        else if (_currentCameraMode == CameraMode.TopDown)
        {
            // 탑다운 카메라
            Vector3 desiredPosition = _target.position + _topDownOffset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _currentVelocity, 0.1f);
        }
    }
}
