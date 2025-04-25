using UnityEngine;
using System;
public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private PlayerRotate _playerRotate;
    [SerializeField] private PlayerFire _playerFire;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CameraFollow _cameraFollow;

    [Header("Player Stats")]
    public float MaxHealth = 100f;
    [SerializeField] private float _currentHealth;
    [SerializeField] private bool _isDead = false;

    public Action OnPlayerDamaged;


    public float CurrentHealth => _currentHealth;
    public bool IsDead => _isDead;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerMove = GetComponent<PlayerMove>();
        _playerRotate = GetComponent<PlayerRotate>();
        _playerFire = GetComponent<PlayerFire>();
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void Start()
    {
        _currentHealth = MaxHealth;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (_isDead) return;
        
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, MaxHealth);
        OnPlayerDamaged?.Invoke();
        if (_currentHealth <= 0)
        {
            Die();
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
    }

    private void Die()
    {
        _isDead = true;
        // TODO: 사망 처리 (애니메이션, 효과 등)
    }
} 