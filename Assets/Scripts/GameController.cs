using System;
using Unity.Collections;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] private GameSettingsSO gameSettingsSO;
    [SerializeField] private Player _player;
    private StateMachine stateMachine;
    bool _running = true;

    private MainMenuState _mainMenuState;
    private GameplayState _gameplayState;
    private GameOverState _gameOverState;

    public bool Running => _running;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        UIManager.Instance.InitializeUIElements(gameSettingsSO.initialPlayerHealth);
        InitializeStateMachine();
        SetState(_mainMenuState);
        UIManager.Instance.MenuUI.OnPlayButtonSubmitted += OnPlayButtonSubmitted;
        UIManager.Instance.GameOverUI.OnRetryButtonSubmitted += OnRetryButtonSubmitted;
    }

    private void InitializeStateMachine()
    {
        stateMachine = new StateMachine();
        _mainMenuState = new MainMenuState(gameSettingsSO, _player);
        _gameplayState = new GameplayState(gameSettingsSO, _player);
        _gameOverState = new GameOverState(gameSettingsSO, _player);
    }
    void Update()
    {
        if (!_running)
            return;

        stateMachine.Update();
    }
    private void OnDestroy()
    {
        UIManager.Instance.MenuUI.OnPlayButtonSubmitted -= OnPlayButtonSubmitted;
        UIManager.Instance.GameOverUI.OnRetryButtonSubmitted -= OnRetryButtonSubmitted;
    }

    public void SetState(IGameState newState)
    {
        stateMachine.SetState(newState);
    }
    private void OnPlayButtonSubmitted() => SetState(_gameplayState);
    private void OnRetryButtonSubmitted() => SetState(_gameplayState);
    public void OnGameOver(int _currentScore)
    {
        UIManager.Instance.GameOverUI.DisplayScore(_currentScore);
        SetState(_gameOverState);
    }
    public void OnGameStarted()
    {
        _running = true;
    }
    public void OnGameStopped()
    {
        _running = false;
    }


}
