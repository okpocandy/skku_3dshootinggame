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
    [SerializeField] private PlayerSO _playerData;

    [Header("Player Stats")]
    [SerializeField] private float _currentHealth;
    [SerializeField] private bool _isDead = false;
    [SerializeField] private float _currentStamina;

    public Action OnPlayerDamaged;
    public Action OnStaminaChanged; // current, max

    public bool IsDead => _isDead;
    public float CurrentHealth => _currentHealth;
    public float CurrentStamina => _currentStamina;
    public float MaxHealth => _playerData.MaxHealth;
    public float MaxStamina => _playerData.MaxStamina;
    public float AddStamina => _playerData.AddStamina;
    public float SubStamina => _playerData.SubStamina;
    public float MinRunStamina => _playerData.MinRunStamina;
    public float SlidingStamina => _playerData.SlidingStamina;
    public Animator _animator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerMove = GetComponent<PlayerMove>();
        _playerRotate = GetComponent<PlayerRotate>();
        _playerFire = GetComponent<PlayerFire>();
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _currentHealth = MaxHealth;
        _currentStamina = MaxStamina;
    }

    public void TakeDamage(float damage)
    {
        if (_isDead) return;
        
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, MaxHealth);
        float weight = Mathf.Clamp01((1-_currentHealth / MaxHealth)*1.5f);
        _animator.SetLayerWeight(1,weight);
        
        // 피격 이벤트 호출
        // UI, 효과등
        OnPlayerDamaged?.Invoke();

        if (_currentHealth <= 0)
        {
            Die();
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
    }

    public void UseStamina(float amount)
    {
        _currentStamina -= amount;
        _currentStamina = Mathf.Clamp(_currentStamina, 0f, MaxStamina);
        OnStaminaChanged?.Invoke();
    }

    public void RecoverStamina(float amount)
    {
        _currentStamina += amount;
        _currentStamina = Mathf.Clamp(_currentStamina, 0f, MaxStamina);
        OnStaminaChanged?.Invoke();
    }

    private void Die()
    {
        _isDead = true;
        // 애니메이션, 효과 등
    }
} 