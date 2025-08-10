using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public MoveCardHandler moveCardHandler;
    public MergeCardHandler mergeCardHandler;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Make sure we have required handlers
        if (moveCardHandler == null) moveCardHandler = GetComponent<MoveCardHandler>();
        if (mergeCardHandler == null) mergeCardHandler = GetComponent<MergeCardHandler>();
    }

    public static Player GetInstance()
    {
        return Instance;
    }
}
