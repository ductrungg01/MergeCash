using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; // Panel pause UI
    private bool isPaused = false; // Trang thai pause

    public static PauseManager instance;

    void Start()
    {
        pausePanel.SetActive(false); // An panel luc dau
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // prevent destroy when scene changes
        }
        else
        {
            Destroy(gameObject); // avoid duplicate managers
        }
    }
    public void TogglePause()
    {
        // Dao trang thai pause
        isPaused = !isPaused;

        if (isPaused)
        {
            pausePanel.SetActive(true); // Hien panel
            Time.timeScale = 0f; // Dung game
            //Time.timeScale = 0f; // Dung game
        }
        else
        {
            pausePanel.SetActive(false); // An panel
            Time.timeScale = 1f; // Chay lai game
            //Time.timeScale = 1f; // Chay lai game
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Reset time
        SceneManager.LoadScene("MainMenu");
        ScoreManager.instance.score = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GridManager.Instance.Start();
        ScoreManager.instance.score = 0;
        TogglePause();
    }
}
