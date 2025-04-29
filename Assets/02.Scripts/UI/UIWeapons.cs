using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWeapons : Singleton<UIWeapons>
{
    [Header("Weapon Stats")]
    public TextMeshProUGUI BombCountText;
    public TextMeshProUGUI BulletCountText;
    public TextMeshProUGUI ReloadingText;
    public Slider ReloadingSlider;

    public void UpdateBombCount(int currentBombCount, int maxBombCount)
    {
        if (BombCountText != null)
        {
            BombCountText.text = $"{currentBombCount} / {maxBombCount}";
        }
    }

    public void UpdateBulletCount(int currentBulletCount, int maxBulletCount)
    {
        if (BulletCountText != null)
        {
            BulletCountText.text = $"{currentBulletCount} / {maxBulletCount}";
        }
    }

    public void ActivateReloading()
    {
        if (ReloadingText != null)
        {
            ReloadingText.gameObject.SetActive(true);
        }
    }

    public void DeactivateReloading()
    {
        if (ReloadingText != null)
        {
            ReloadingText.gameObject.SetActive(false);
        }
    }

    public void ActivateReloadingSlider()
    {
        if (ReloadingSlider != null)
        {
            ReloadingSlider.gameObject.SetActive(true);
        }
    }

    public void DeactivateReloadingSlider()
    {
        if (ReloadingSlider != null)
        {
            ReloadingSlider.gameObject.SetActive(false);
        }
    }

    public void UpdateReloadingSlider(float currentReloadingTime, float maxReloadingTime)
    {
        if (ReloadingSlider != null)
        {
            ReloadingSlider.value = currentReloadingTime / maxReloadingTime;
        }
    }
} 