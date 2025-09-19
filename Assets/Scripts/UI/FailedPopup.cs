using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FailedPopup : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text bestScoreText;   
    [SerializeField] private TMP_Text currentScoreText; 
    [SerializeField] private Button retryButton;       

    private int currentScore;
    private int bestScore;

    private void Awake()
    {
        if (retryButton == null)
            retryButton = GetComponentInChildren<Button>();

        retryButton.onClick.AddListener(OnRetryClicked);

        gameObject.SetActive(false);
    }

    public void Setup(int score, int highestScore)
    {
        currentScore = score;
        bestScore = highestScore;

        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();

        if (currentScoreText != null)
            currentScoreText.text = currentScore.ToString();

        gameObject.SetActive(true);
        transform.SetAsLastSibling(); 
    }

    private void OnRetryClicked()
    {
        Player.Instance.ProcessReloadScene();
    }
}
