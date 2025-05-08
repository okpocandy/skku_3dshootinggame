using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIBlood : MonoBehaviour
{
    public Image BloodImage;
    public float FadeOutTime = 0.5f;
    public float FadeInTime = 1f;

    public Player Player;
    private Coroutine _currentFadeCoroutine;

    private void Start()
    {
        Player.OnPlayerDamaged += BloodEffect;
    }

    public void BloodEffect()
    {
        if (_currentFadeCoroutine != null)
        {
            StopCoroutine(_currentFadeCoroutine);
        }

        BloodImage.color = new Color(1f, 1f, 1f, 1f);
        
        _currentFadeCoroutine = StartCoroutine(FadeOutBloodCoroutine());
    }

    public void FadeOutBlood()
    {
        StartCoroutine(FadeOutBloodCoroutine());
    }

    private IEnumerator FadeOutBloodCoroutine()
    {
        Color color = BloodImage.color;
        while(color.a > 0f)
        {
            color.a -= Time.deltaTime / FadeOutTime;
            BloodImage.color = color;
            yield return null;
        }
        color.a = 0f;
        BloodImage.color = color;
        _currentFadeCoroutine = null;
    }

    public void FadeInBlood()
    {
        StartCoroutine(FadeInBloodCoroutine());
    }

    private IEnumerator FadeInBloodCoroutine()
    {
        Color color = BloodImage.color;
        while(color.a < 1f)
        {
            color.a += Time.deltaTime * FadeInTime;
            BloodImage.color = color;
            yield return null;
        }
        color.a = 1f;
        BloodImage.color = color;
    }
}
