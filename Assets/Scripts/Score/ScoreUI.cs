using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text highestScoreText;

    private void OnEnable()
    {
        ScoreManager.Instance.OnScoreChanged += UpdateScoreUI;
    }

    private void OnDisable()
    {
        ScoreManager.Instance.OnScoreChanged -= UpdateScoreUI;
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (currentScoreText != null) 
            currentScoreText.text = ScoreManager.Instance.CurrentScore.ToString();
        
        if (highestScoreText != null)
            highestScoreText.text = ScoreManager.Instance.HighestScore.ToString();
    }
}
