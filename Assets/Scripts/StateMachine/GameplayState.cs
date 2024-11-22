using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameplayState : GameStateBase
{
    public GameplayState(GameSettingsSO gameSettingsSO, Player player) : base(gameSettingsSO, player) { }

    private float _enemySpawnTimer = 0.0f;
    private int _currentScore;

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Gameplay State");
        _player.Initialize(_gameSettingsSO.spawnPosition);
        _player.OnDie += OnPlayerDie;
        _player.OnHealthUpdated += OnHealthUpdated;
        UIManager.Instance.GameplayUI.Show();
        GameController.Instance.OnGameStarted();
    }

    public override void Update()
    {
        base.Update();
        _enemySpawnTimer += Time.deltaTime;
        if (_enemySpawnTimer >= _gameSettingsSO.enemySpawnInterval)
        {
            SpawnEnemy();
            _enemySpawnTimer = 0.0f;
        }

    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exiting Gameplay State");
        _player.OnDie -= OnPlayerDie;
        _player.OnHealthUpdated -= OnHealthUpdated;
        UIManager.Instance.GameplayUI.Hide();
        GameController.Instance.OnGameStopped();
    }
    private void SpawnEnemy()
    {
        var enemy = EnemyPoolManager.Instance.Get(EnemyType.Basic);
        enemy.transform.position = new Vector3(
            UnityEngine.Random.Range(_gameSettingsSO.spawnAreaMin.x, _gameSettingsSO.spawnAreaMax.x),
            UnityEngine.Random.Range(_gameSettingsSO.spawnAreaMin.y, _gameSettingsSO.spawnAreaMax.y),
            0
        );
        enemy.OnUpdateScoreRequest += UpdateScore;
    }
    private void UpdateScore(int scoreToAdd)
    {
        _currentScore += scoreToAdd;
        UIManager.Instance.RefreshScore(_currentScore);
    }
    private void OnPlayerDie()
    {
        GameController.Instance.OnGameOver(_currentScore);
    }
    private void OnHealthUpdated(int health)
    {
        UIManager.Instance.UpdateHealth(health);
    }
}

