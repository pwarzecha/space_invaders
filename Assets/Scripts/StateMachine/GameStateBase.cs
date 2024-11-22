using System;
using UnityEngine;
public abstract class GameStateBase : IGameState
{
    protected GameSettingsSO _gameSettingsSO;
    protected Player _player;
    public GameStateBase(GameSettingsSO gameSettingsSO, Player player)
    {
        _gameSettingsSO = gameSettingsSO;
        _player = player;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
