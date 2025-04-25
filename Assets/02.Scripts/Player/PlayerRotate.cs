using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150f;
    public float MaxVerticalAngle = 80f;
    public float MinVerticalAngle = -80f;
    private CameraFollow _cameraFollow;

    private float _rotationX = 0;
    private float _rotationY = 0;

    private void Start()
    {
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void Update()
    {
        if (_cameraFollow == null) return;

        switch (_cameraFollow.CurrentCameraMode)
        {
            case CameraMode.FirstPerson:
                // 1인칭: 카메라 회전과 동기화
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                _rotationX += mouseX * RotationSpeed * Time.deltaTime;
                _rotationY -= mouseY * RotationSpeed * Time.deltaTime;
                _rotationY = Mathf.Clamp(_rotationY, MinVerticalAngle, MaxVerticalAngle);

                transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
                break;

            case CameraMode.ThirdPerson:
                // 3인칭: 카메라 방향으로 회전
                Vector3 cameraForward = Camera.main.transform.forward;
                cameraForward.y = 0;
                if (cameraForward != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
                }
                break;

            case CameraMode.TopDown:
                // 탑다운: 마우스 커서 방향으로 회전
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane groundPlane = new Plane(Vector3.up, transform.position);
                float rayDistance;
                if (groundPlane.Raycast(ray, out rayDistance))
                {
                    Vector3 targetPoint = ray.GetPoint(rayDistance);
                    Vector3 direction = (targetPoint - transform.position).normalized;
                    direction.y = 0;
                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
                    }
                }
                break;
        }
    }
}
