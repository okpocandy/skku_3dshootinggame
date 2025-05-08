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
}
