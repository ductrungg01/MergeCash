using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int CurrentScore { get; private set; }
    public int HighestScore { get; private set; }

    public event Action OnScoreChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        HighestScore = PlayerPrefs.GetInt("HighestScore", 0);
    }

    public void AddScore(int amount)
    {
        CurrentScore += amount;

        if (CurrentScore > HighestScore)
        {
            HighestScore = CurrentScore;
            PlayerPrefs.SetInt("HighestScore", HighestScore);
            PlayerPrefs.Save();
        }

        OnScoreChanged?.Invoke();
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        OnScoreChanged?.Invoke();
    }
}
