using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : GameStateBase
{
    public MainMenuState(GameDataSO gameDataSO, Player player) : base(gameDataSO, player) { }

    public override void Enter()
    {
        Debug.Log("Entered Main Menu State");
        UIManager.Instance.MenuUI.Show();
    }

    public override void Exit()
    {
        Debug.Log("Exiting Main Menu State");
        UIManager.Instance.MenuUI.Hide();
    }
}
