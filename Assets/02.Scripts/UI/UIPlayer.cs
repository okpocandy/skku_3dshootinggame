using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : Singleton<UIPlayer>
{
    [Header("Player Stats")]
    public Slider StaminaSlider;
    public Slider HealthSlider;

    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        if (_player != null)
        {
            _player.OnPlayerDamaged += UpdatePlayerUI;
            _player.OnStaminaChanged += UpdatePlayerUI;
        }
    }

    public void UpdatePlayerUI()
    {
        if (_player == null) return;

        HealthSlider.value = _player.CurrentHealth / _player.MaxHealth;
        StaminaSlider.value = _player.CurrentStamina / _player.MaxStamina;
    }
} 