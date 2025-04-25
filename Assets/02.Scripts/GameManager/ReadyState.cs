using UnityEngine;

public class ReadyState : IGameState
{
    private GameManager _gameManager;


    public ReadyState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("Enter Ready State");
        // TODO: 레디 상태 UI 표시
        // 게임을 정지 시킨다.
        Time.timeScale = 0f;
    }

    public void Update()
    {

    }

    public void Exit()
    {
        Debug.Log("Exit Ready State");
    }
} 