using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : GameStateBase
{
    public GameOverState(GameDataSO gameDataSO, Player player) : base(gameDataSO, player) { }

    public override void Enter()
    {
        Debug.Log("Entered Game Over State");
        UIManager.Instance.GameOverUI.Show();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Game Over State");
        UIManager.Instance.GameOverUI.Hide();
    }
}

