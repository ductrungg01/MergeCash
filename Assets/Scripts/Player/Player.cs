using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public static Player GetInstance()
    {
        return Instance;
    }

    public bool IsGameOver()
    {
        if (GridManager.Instance.IsGridOverCapacity()) return true;
        return false;
    }

    public void ProcessGameOver()
    {
        UIManager.Instance.ShowFailedPopup(true);
    }

    public void ProcessReloadScene()
    {
        Debug.Log("Reloading scene: " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ProcessDelete()
    {
        Debug.Log("[ProcessDelete]");

        GridManager.Instance.RemoveAllLastCard();
    }

    public void ProcessSuffle()
    {
        Debug.Log("[ProcessSuffle]");
        GridManager.Instance.SuffleCards();
    }
}
