using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuUI : MonoBehaviour
{
    public Action OnPlayButtonSubmitted;
    public void Initialize()
    {
        Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void OnPlayButton()
    {
        OnPlayButtonSubmitted?.Invoke();
    }
    public void OnExitButton()
    {
        Application.Quit();
    }

}