using System;
using UnityEngine;
public abstract class GameStateBase : IGameState
{
    protected GameDataSO _gameDataSO;
    protected Player _player;
    public GameStateBase(GameDataSO gameDataSO, Player player)
    {
        this._gameDataSO = gameDataSO;
        _player = player;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
