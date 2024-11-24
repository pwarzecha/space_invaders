using System;
using Unity.Collections;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] private GameDataSO gameSettingsSO;
    [SerializeField] private Player _player;
    [SerializeField] private BackgroundScroller _backgroundScroller;
    [SerializeField] private WaveManager waveManager;
    private StateMachine stateMachine;
    private MainMenuState _mainMenuState;
    private GameplayState _gameplayState;
    private GameOverState _gameOverState;
    bool _running = false;

    public bool Running => _running;
    public GameDataSO GameSettingsSO => gameSettingsSO;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        UIManager.Instance.InitializeUIElements();
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
        stateMachine.Update();

        if (!_running)
            return;

        _backgroundScroller.UpdateBackground();
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
