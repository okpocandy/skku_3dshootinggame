using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_OptionPopup : Singleton<UI_OptionPopup>
{
    public UI_CreditPopup UI_CreditPopup;
    
    public Stack<GameObject> _popupStack = new Stack<GameObject>();
    public GameObject _currentPopup;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    private void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_popupStack.Count > 0)
            {
                ClosePopup();
            }
            else
            {
                GameManager.Instance.ChangeState(GameState.Pause);
            }
        }
        */
    }

    public void OnClickContinueButton()
    {
        GameManager.Instance.ChangeState(GameState.Play);
    }
    public void OnClickRetryButton()
    {
        SceneManager.LoadScene(0);
    }
    public void OnClickExitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OnClickCreditButton()
    {
        PopupManager.Instance.OpenPopup(UI_CreditPopup.gameObject);
    }

    public void OpenPopup(GameObject popup)
    {
        // 이전 팝업이 있다면 비활성화
        if (_currentPopup != null)
        {
            _currentPopup.SetActive(false);
        }

        _popupStack.Push(popup);
        popup.SetActive(true);
        _currentPopup = popup;
    }

    public void ClosePopup()
    {
        if (_popupStack.Count > 0)
        {
            // 현재 팝업 비활성화
            GameObject currentPopup = _popupStack.Pop();
            currentPopup.SetActive(false);

            // 이전 팝업이 있다면 다시 활성화
            if (_popupStack.Count > 0)
            {
                _currentPopup = _popupStack.Peek();
                _currentPopup.SetActive(true);
            }
            else
            {
                _currentPopup = null;
                GameManager.Instance.ChangeState(GameState.Play);
            }
        }
    }
    
        
}
