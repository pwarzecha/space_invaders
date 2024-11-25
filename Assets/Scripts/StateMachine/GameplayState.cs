using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class GameplayState : GameStateBase
{
    public GameplayState(GameDataSO gameDataSO, Player player) : base(gameDataSO, player) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Gameplay State");
        _player.Initialize(gameDataSO.spawnPosition);
        _player.OnDie += OnPlayerDie;
        _player.OnHealthUpdated += OnHealthUpdated;
        UIManager.Instance.GameplayUI.Initialize(_player.PlayerDataSO.maxHealth);
        UIManager.Instance.GameplayUI.Show();
        GameController.Instance.OnGameStarted();
    }

    public override void Update()
    {

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
        foreach (PowerUpType type in System.Enum.GetValues(typeof(PowerUpType)))
        {
            PowerUpPoolManager.Instance.ReturnAll(type);
        }
        GameController.Instance.OnGameStopped();
    }

    private void OnPlayerDie()
    {
        GameController.Instance.OnGameOver();
    }
    private void OnHealthUpdated(int health)
    {
        UIManager.Instance.UpdateHealth(health);
    }
}

