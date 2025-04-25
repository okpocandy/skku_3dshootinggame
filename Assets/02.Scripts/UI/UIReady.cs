using System.Collections;
using TMPro;
using UnityEngine;

public class UIReady : Singleton<UIReady>
{
    public TextMeshProUGUI ReadyText;
    private bool _isReady = false;
    public bool IsReady => _isReady;
    //private bool _isGameStart = false;

    private void Update()
    {
        if(!_isReady)
        {
            if(Input.GetMouseButtonDown(0))
            {
                StartCoroutine(ReadyTextCoroutine());
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                StartCoroutine(GameStartCoroutine());
                GameManager.Instance.ChangeState(GameState.Play);
            }
        }

    }

    private IEnumerator ReadyTextCoroutine()
    {  
        yield return new WaitForSecondsRealtime(1f);
        if (ReadyText != null)
        {
            ReadyText.text = "클릭하여 시작";
        }
        _isReady = true;
    }

    private IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        gameObject.SetActive(false);
    }
}
