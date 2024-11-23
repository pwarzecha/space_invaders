using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class GameplayState : GameStateBase
{
    public GameplayState(GameSettingsSO gameSettingsSO, Player player) : base(gameSettingsSO, player) { }

    private bool _canSpawnEnemies;
    private int _enemySpawnDelay = 1500;
    private float _enemySpawnTimer = 0.0f;
    private int _currentScore;

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Gameplay State");
        _canSpawnEnemies = false;
        _currentScore = 0;
        _player.Initialize(_gameSettingsSO.spawnPosition);
        _player.OnDie += OnPlayerDie;
        _player.OnHealthUpdated += OnHealthUpdated;
        UIManager.Instance.GameplayUI.Initialize(_player.PlayerSettingsSO.maxHealth);
        UIManager.Instance.GameplayUI.Show();
        GameController.Instance.OnGameStarted();
        WaitForInitialSpawnDelay();
    }

    public override void Update()
    {
        base.Update();
        if (!_canSpawnEnemies)
            return;

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
        foreach (EnemyType type in System.Enum.GetValues(typeof(EnemyType)))
        {
            EnemyPoolManager.Instance.ReturnAll(type);
        }
        foreach (ProjectileType type in System.Enum.GetValues(typeof(ProjectileType)))
        {
            ProjectilePoolManager.Instance.ReturnAll(type);
        }
        GameController.Instance.OnGameStopped();
    }
    private void SpawnEnemy()
    {
        Vector3 screenMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 screenMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        EnemyType randomEnemyType = _gameSettingsSO.GetRandomEnemyType();
        var enemy = EnemyPoolManager.Instance.Get(randomEnemyType);

        enemy.transform.position = new Vector3(
               UnityEngine.Random.Range(screenMin.x, screenMax.x),
               screenMax.y, 
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

    private async Task WaitForInitialSpawnDelay()
    {
        await Task.Delay(_enemySpawnDelay);
        _canSpawnEnemies = true;
    }
}

