using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    
    public Slider StaminaSlider;

    public void UpdateStamina(float currentStamina, float maxStamina)
    {
        StaminaSlider.value = currentStamina / maxStamina;
    }

}
