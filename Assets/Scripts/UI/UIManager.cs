using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MenuUI _menuUI;
    [SerializeField] private GameplayUI _gameplayUI;
    [SerializeField] private GameOverUI _gameOverUI;

    public MenuUI MenuUI  => _menuUI; 
    public GameplayUI GameplayUI => _gameplayUI; 
    public GameOverUI GameOverUI => _gameOverUI; 

    public void InitializeUIElements(int initialPlayerHealth)
    {
        _menuUI.Initialize();
        _gameplayUI.Initialize(initialPlayerHealth);
        _gameOverUI.Initialize();
    }

    public void RefreshScore(int newScore)
    {
        _gameplayUI.DisplayScore(newScore);
    }

    public void UpdateHealth(int h)
    {
        _gameplayUI.UpdateHealth(h);
    }

}