using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Panel pause UI
    private bool isPaused = false; // Trang thai pause

    void Start()
    {
        pausePanel.SetActive(false); // An panel luc dau
    }

    public void TogglePause()
    {
        // Dao trang thai pause
        isPaused = !isPaused;

        if (isPaused)
        {
            pausePanel.SetActive(true); // Hien panel
            Time.timeScale = 0f; // Dung game
        }
        else
        {
            pausePanel.SetActive(false); // An panel
            Time.timeScale = 1f; // Chay lai game
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Reset time
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
