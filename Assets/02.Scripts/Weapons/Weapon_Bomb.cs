using UnityEngine;

public class Weapon_Bomb : MonoBehaviour
{
    [Header("폭탄")]
    public float MinThrowPower = 15f;
    public float MaxThrowPower = 45f;
    public float ThrowPowerStep = 10f;
    [SerializeField]
    private float _currentThrowPower = 0f;

    public int MaxBombCount = 3;
    private int _currentBombCount = 0;

    private Camera _mainCamera;
    private CameraFollow _cameraFollow;
    public Animator _animator;
    public GameObject FirePosition;
    private Transform _playerTransform;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
        _playerTransform = transform.root; // Player transform 가져오기
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentThrowPower = MinThrowPower;
        _currentBombCount = MaxBombCount;
        UIWeapons.Instance.UpdateBombCount(_currentBombCount, MaxBombCount);
    }


    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            _currentThrowPower += ThrowPowerStep * Time.deltaTime;
            _currentThrowPower = Mathf.Clamp(_currentThrowPower, MinThrowPower, MaxThrowPower);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(_currentBombCount > 0)
            {
                _animator.SetTrigger("Throw");
                // 이벤트로 호출
                //ThrowBomb();
            }
            else
            {
                Debug.Log("폭탄이 부족합니다.");
            }
        }
    }

    public void ThrowBomb()
    {
        _currentBombCount--;
        UIWeapons.Instance.UpdateBombCount(_currentBombCount, MaxBombCount);

        Bomb bomb = WeaponPool.Instance.GetBomb();
        if(bomb == null)
        {
            Debug.Log("폭탄이 부족합니다.");
            return;
        }
        bomb.transform.position = FirePosition.transform.position;

        Vector3 fireDirection = _cameraFollow.CurrentCameraMode == CameraMode.FirstPerson 
            ? Camera.main.transform.forward  // FPS: 카메라 방향
            : _playerTransform.forward;      // TPS: 플레이어 캐릭터 방향

        Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
        bombRigidbody.AddForce(fireDirection * _currentThrowPower, ForceMode.Impulse);
        bombRigidbody.AddTorque(Vector3.one);

        _currentThrowPower = MinThrowPower;
    }
}
