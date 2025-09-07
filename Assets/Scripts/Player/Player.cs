using UnityEngine;

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
}
