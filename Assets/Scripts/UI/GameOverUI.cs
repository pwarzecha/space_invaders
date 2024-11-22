using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    public Action OnRetryButtonSubmitted;
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
    public void DisplayScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
    public void OnRetryButton()
    {
        OnRetryButtonSubmitted?.Invoke();
    }

}
