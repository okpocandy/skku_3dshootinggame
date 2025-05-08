using UnityEngine;

public class PauseState : IGameState
{
    private GameManager _gameManager;

    public PauseState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Enter()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        // 옵션 팝업을 스택에 추가
        PopupManager.Instance.OpenPopup(_gameManager.UI_OptionPopup);
    }

    public void Update()
    {
    }

    public void Exit()
    {
        Debug.Log("Exit Pause State");
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
