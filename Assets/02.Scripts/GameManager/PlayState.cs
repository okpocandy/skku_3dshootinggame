using UnityEngine;

public class PlayState : IGameState
{
    private GameManager _gameManager;

    public PlayState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("Enter Play State");
        // TODO: 게임 시작 처리
        // TODO: 타이머 시작
        Time.timeScale = 1f;
    }

    public void Update()
    {
        // TODO: 게임 로직 업데이트
        if (Input.GetKeyDown(KeyCode.O))
        {
            _gameManager.ChangeState(GameState.GameOver);
        }
    }

    public void Exit()
    {
        Debug.Log("Exit Play State");
        // TODO: 게임 종료 처리
    }
} 