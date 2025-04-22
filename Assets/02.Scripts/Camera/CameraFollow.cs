using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform FPSTarget;
    public Transform TPSTarget;
    public Transform QuaterTarget;

    [SerializeField]
    private Transform _target;

    private void Start()
    {
        _target = FPSTarget;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            _target = FPSTarget;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            _target = TPSTarget;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            _target = QuaterTarget;
        }
        transform.position = _target.position;
    }
}
