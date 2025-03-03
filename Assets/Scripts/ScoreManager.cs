using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        UpdateScoreText(); // Update score at the start
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText(); 
    }

    void UpdateScoreText()
    {
        scoreText.text = "Coins Collected: " + score; // Update the score text
    }
}