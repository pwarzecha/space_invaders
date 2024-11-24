using System;
using Unity.Collections;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] private GameDataSO gameDataSO;
    [SerializeField] private Player _player;
    [SerializeField] private BackgroundScroller _backgroundScroller;
    [SerializeField] private WaveManager _waveManager;
    private StateMachine stateMachine;
    private MainMenuState _mainMenuState;
    private GameplayState _gameplayState;
    private GameOverState _gameOverState;
    private bool _running = false;
    private int _currentScore;
    public bool Running => _running;
    public GameDataSO GameDataSO => gameDataSO;

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
        _mainMenuState = new MainMenuState(gameDataSO, _player);
        _gameplayState = new GameplayState(gameDataSO, _player);
        _gameOverState = new GameOverState(gameDataSO, _player);
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

    public void OnGameOver()
    {
        UIManager.Instance.GameOverUI.DisplayScore(_currentScore);
        SetState(_gameOverState);
    }
    public void OnGameStarted()
    {
        _running = true;
        _currentScore = 0;
        _waveManager.OnUpdateScoreRequest += UpdateScore;
        _waveManager.StartHandlingWaves();
    }
    public void OnGameStopped()
    {
        _waveManager.OnUpdateScoreRequest -= UpdateScore;
        _waveManager.StopWaves();
        _running = false;
    }
    private void UpdateScore(int scoreToAdd)
    {
        _currentScore = Mathf.Max(0, _currentScore + scoreToAdd);
        UIManager.Instance.RefreshScore(_currentScore);
    }

}
