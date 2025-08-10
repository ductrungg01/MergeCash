using UnityEngine;

public class MergeCardHandler : MonoBehaviour
{
    private GridManager gridManager;

    private void Start()
    {
        gridManager = GridManager.GetInstance();
    }

    public void TryMergeColumn(int col)
    {
        for (int row = 0; row < gridManager.maxRows - 1; row++)
        {
            Card card1 = gridManager.GetCardAt(col, row);
            Card card2 = gridManager.GetCardAt(col, row + 1);
            if (card1 != null && card2 != null && card1.value == card2.value)
            {
                card1.SetValue(card1.value * 2);
                Destroy(card2.gameObject);
                ShiftColumnDown(col, row + 1);
                row--;
            }
        }
    }

    private void ShiftColumnDown(int col, int startRow)
    {
        for (int row = startRow; row < gridManager.maxRows - 1; row++)
        {
            gridManager.SetCardAt(col, row, gridManager.GetCardAt(col, row + 1));
            if (gridManager.GetCardAt(col, row) != null)
                gridManager.GetCardAt(col, row).SetGridPosition(col, row);
        }
        gridManager.SetCardAt(col, gridManager.maxRows - 1, null);
    }
}
