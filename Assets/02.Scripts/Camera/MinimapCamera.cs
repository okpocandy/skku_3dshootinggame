using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target;
    public float YOffset = 10f;
    public float MaxOrthographicSize = 15f;
    public float MinOrthographicSize = 5f;
    public float ZoomSpeed = 1f;

    private Camera _camera;

    private void Start() {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector3 newPosition = Target.position;
        newPosition.y += YOffset;

        transform.position = newPosition;

        Vector3 newEulerAngles = Target.eulerAngles;
        newEulerAngles.x = 90f;
        newEulerAngles.z = 0f;


        transform.eulerAngles = newEulerAngles;
    }

    public void OnZoonInButtonClick()
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - ZoomSpeed, MinOrthographicSize, MaxOrthographicSize);
    }

    public void OnZoomOutButtonClick()
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + ZoomSpeed, MinOrthographicSize, MaxOrthographicSize);
    }
}
