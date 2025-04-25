using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    // 스테미나
    public Slider StaminaSlider;
    // 체력
    public Slider HealthSlider;
    // 폭탄 개수
    public TextMeshProUGUI BombCountText;
    // 총알 개수
    public TextMeshProUGUI BulletCountText;
    // 재장전 텍스트
    public TextMeshProUGUI ReloadingText;
    // 재장전 슬라이더
    public Slider ReloadingSlider;

    public Player Player;

    private void Start()
    {
        Player.OnPlayerDamaged += UpdateHealth;
    }

    public void UpdateHealth()
    {
        HealthSlider.value = Player.CurrentHealth / Player.MaxHealth;
    }

    public void UpdateStamina(float currentStamina, float maxStamina)
    {
        StaminaSlider.value = currentStamina / maxStamina;
    }

    public void UpdateBombCount(int currentBombCount, int maxBombCount)
    {
        BombCountText.text = $"{currentBombCount} / {maxBombCount}";
    }

    public void UpdateBulletCount(int currentBulletCount, int maxBulletCount)
    {
        BulletCountText.text = $"{currentBulletCount} / {maxBulletCount}";
    }

    public void ActivateReloading()
    {
        ReloadingText.gameObject.SetActive(true);
    }

    public void DeactivateReloading()
    {
        ReloadingText.gameObject.SetActive(false);
    }

    public void ActivateReloadingSlider()
    {
        ReloadingSlider.gameObject.SetActive(true);
    }

    public void DeactivateReloadingSlider()
    {
        ReloadingSlider.gameObject.SetActive(false);
    }

    public void UpdateReloadingSlider(float currentReloadingTime, float maxReloadingTime)
    {
        ReloadingSlider.value = currentReloadingTime / maxReloadingTime;
    }

}
