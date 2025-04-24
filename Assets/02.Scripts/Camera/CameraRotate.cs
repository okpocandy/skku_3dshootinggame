using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float RotationSpeed = 150f;
    public float MaxVerticalAngle = 80f;
    public float MinVerticalAngle = -80f;

    // 카메라 각도는 0도에서부터 시작한다고 기준을 세운다.
    private float _rotationX = 0f;
    private float _rotationY = 0f;
    
    // 반동 효과
    [SerializeField]
    private float _recoilIntensity = 1f;

    private CameraFollow _cameraFollow;
    
    private void Start()
    {
        Gun gun = FindObjectOfType<Gun>();
        if (gun != null)
        {
            //gun.OnFire += Recoil;
        }
        
        _cameraFollow = GetComponent<CameraFollow>();
    }

    // 카메라 회전 스크립트
    // 목표: 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.
    private void Update() 
    {
        if(_cameraFollow.CurrentCameraMode == CameraMode.TopDown)
        {
            transform.rotation = Quaternion.Euler(30, 0, 0);
            return;
        }
            
        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 회전값 계산
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY -= mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, MinVerticalAngle, MaxVerticalAngle);

        // 카메라 회전 적용
        transform.eulerAngles =  new Vector3(_rotationY, _rotationX, 0);
    }

    private void Recoil()
    {
        _rotationY -= _recoilIntensity;
    }

}
