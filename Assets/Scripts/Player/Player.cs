using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private PlayerItemManager playerItemManager;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        playerItemManager = GetComponent<PlayerItemManager>();
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

    public void ProcessDelete(bool useActionItem = true)
    {
        Debug.Log("[ProcessDelete]");

        if (useActionItem)
        {
            SetRemainDeleteAction(GetRemainDeleteAction() - 1);
        }

        GridManager.Instance.RemoveAllLastCard();
    }

    public void ProcessShuffle(bool useActionItem = true)
    {
        Debug.Log("[ProcessShuffle]");

        if (useActionItem)
        {
            SetRemainShuffleAction(GetRemainShuffleAction() - 1);
        }

        GridManager.Instance.ShuffleCards();
    }

    public int GetRemainDeleteAction()
    {
        return playerItemManager.GetRemainDeleteAction();
    }

    public int GetRemainShuffleAction()
    {
        return playerItemManager.GetRemainShuffleAction();
    }

    public void SetRemainDeleteAction(int newValue)
    {
        playerItemManager.SetRemainDeleteAction(newValue);
    }

    public void SetRemainShuffleAction(int newValue)
    {
        playerItemManager.SetRemainShuffleAction(newValue);
    }
}
