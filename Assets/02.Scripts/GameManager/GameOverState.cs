using UnityEngine;

public class GameOverState : IGameState
{
    private GameManager _gameManager;

    public GameOverState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("Enter GameOver State");
        // TODO: 게임오버 UI 표시
        _gameManager.UIGameOver.gameObject.SetActive(true);
        
        Time.timeScale = 0f;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _gameManager.ChangeState(GameState.Ready);
        }
    }

    public void Exit()
    {
        Debug.Log("Exit GameOver State");
        // TODO: 게임오버 UI 숨기기
    }
} 