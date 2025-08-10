using UnityEngine;
using UnityEngine.InputSystem; // Required for new Input System

public class GridHoverDebugger : MonoBehaviour
{
    private GridManager gridManager;

    void Start()
    {
        gridManager = GridManager.GetInstance();
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            int col = gridManager.GetNearestColumn(mousePos);
            int row = gridManager.GetNearestRow(mousePos);

            Debug.Log($"[Hover Debug] MousePos: {mousePos}, Col: {col}, Row: {row}");
        }
    }
}
