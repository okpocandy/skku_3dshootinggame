using UnityEngine;
using System.Collections.Generic;

public enum GameState
{
    Ready,
    Play,
    GameOver,
}

public class GameManager : Singleton<GameManager>
{
    private Dictionary<GameState, IGameState> _states;
    private IGameState _currentState;
    private GameState _currentStateType;

    public UIGameOver UIGameOver;

    private void Awake()
    {
        InitializeStates();
        ChangeState(GameState.Ready);
    }

    private void InitializeStates()
    {
        _states = new Dictionary<GameState, IGameState>
        {
            { GameState.Ready, new ReadyState(this) },
            { GameState.Play, new PlayState(this) },
            { GameState.GameOver, new GameOverState(this) }
        };
    }

    private void Update()
    {
        _currentState?.Update();
    }

    public void ChangeState(GameState newState)
    {
        _currentState?.Exit();
        _currentStateType = newState;
        _currentState = _states[newState];
        _currentState.Enter();
    }

    public GameState GetCurrentState()
    {
        return _currentStateType;
    }
} 