using Cysharp.Threading.Tasks;
using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class GameplayState : GameStateBase
{
    public GameplayState(GameDataSO gameDataSO, Player player, BackgroundScroller backgroundScroller,
        WaveManager waveManager, Camera mainCamera) : base(gameDataSO, player)
    {
        _backgroundScroller = backgroundScroller;
        _waveManager = waveManager;
        _mainCamera = mainCamera;
    }

    private BackgroundScroller _backgroundScroller;
    private WaveManager _waveManager;
    private Camera _mainCamera;
    private int _currentScore;
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Gameplay State");
        UIManager.Instance.GameplayUI.Initialize(_player.PlayerDataSO.maxHealth);
        UIManager.Instance.GameplayUI.Show();
        _currentScore = 0;
        _player.Initialize(_gameDataSO.spawnPosition);
        _player.OnDie += OnGameOver;
        _player.OnHealthUpdated += OnHealthUpdated;
        _player.OnGetDamage += ShakeCamera;
        _waveManager.OnUpdateScoreRequest += UpdateScore;
        _waveManager.StartHandlingWaves();
        GameController.Instance.OnGameStarted();
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exiting Gameplay State");
        UIManager.Instance.GameplayUI.Hide();
        _player.OnGetDamage -= ShakeCamera;
        _player.OnDie -= OnGameOver;
        _player.OnHealthUpdated -= OnHealthUpdated;
        _waveManager.OnUpdateScoreRequest -= UpdateScore;
        _waveManager.StopWaves();
        EnemyPoolManager.Instance.ReturnAll();
        ProjectilePoolManager.Instance.ReturnAll();
        PowerUpPoolManager.Instance.ReturnAll();
        GameController.Instance.OnGameStopped();
    }

    private void OnGameOver()
    {
        UIManager.Instance.GameOverUI.DisplayScore(_currentScore);
        GameController.Instance.OnGameOver();
    }

    private void OnHealthUpdated(int health)
    {
        UIManager.Instance.UpdateHealth(health);
    }
    private void UpdateScore(int scoreToAdd)
    {
        _currentScore = Mathf.Max(0, _currentScore + scoreToAdd);
        UIManager.Instance.RefreshScore(_currentScore);
    }
    private void ShakeCamera() => Shake();
    private Sequence Shake(float startDelay = 0)
    {
        return Tween.ShakeCamera(_mainCamera, _gameDataSO.cameraShakeStrength, startDelay: startDelay);
    }
}

