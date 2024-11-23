using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private TextMeshProUGUI scoreText;
    public void Initialize(int maxHealth)
    {
        scoreText.text = "0";

        if (hearts.Length < maxHealth)
        {
            Debug.LogError("Not enough heart references provided in the array");
            return;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < maxHealth);
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentHealth);
        }
    }
    public void DisplayScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
