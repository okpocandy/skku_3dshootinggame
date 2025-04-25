using System;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // - 발사 위치
    public GameObject FirePosition;
    [Header("폭탄")]
    // 던지는 힘
    public float MinThrowPower = 15f;
    public float MaxThrowPower = 45f;
    public float ThrowPowerStep = 10f;
    [SerializeField]
    private float _currentThrowPower = 0f;

    public int MaxBombCount = 3;
    private int _currentBombCount = 0;

    private Camera _mainCamera;
    private Gun _gun;
    private CameraFollow _cameraFollow;
    private void Start() 
    {
        _mainCamera = Camera.main;
        _gun = GetComponent<Gun>();
        _currentThrowPower = MinThrowPower;
        _currentBombCount = MaxBombCount;
        UIManager.Instance.UpdateBombCount(_currentBombCount, MaxBombCount);

        _cameraFollow = Camera.main.GetComponent<CameraFollow>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Bomb();
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Bomb()
    {
        if(Input.GetMouseButton(1))
        {
            _currentThrowPower += ThrowPowerStep * Time.deltaTime;
            _currentThrowPower = Mathf.Clamp(_currentThrowPower, MinThrowPower, MaxThrowPower);
        }
        if (Input.GetMouseButtonUp(1))
        {
            if(_currentBombCount > 0)
            {
                _currentBombCount--;
                UIManager.Instance.UpdateBombCount(_currentBombCount, MaxBombCount);

                Bomb bomb = WeaponPool.Instance.GetBomb();
                if(bomb == null)
                {
                    Debug.Log("폭탄이 부족합니다.");
                    return;
                }
                bomb.transform.position = FirePosition.transform.position;

                Vector3 fireDirection = _cameraFollow.CurrentCameraMode == CameraMode.FirstPerson 
                    ? Camera.main.transform.forward  // FPS: 카메라 방향
                    : transform.forward;            // TPS: 플레이어 캐릭터 방향

                Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
                bombRigidbody.AddForce(fireDirection * _currentThrowPower, ForceMode.Impulse);
                bombRigidbody.AddTorque(Vector3.one);

                _currentThrowPower = MinThrowPower;
            }
            else
            {
                Debug.Log("폭탄이 부족합니다.");
            }
        }
    }
}
