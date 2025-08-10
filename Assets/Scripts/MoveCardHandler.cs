using UnityEngine;

public class MoveCardHandler : MonoBehaviour
{
    private GridManager gridManager;

    private void Start()
    {
        gridManager = GridManager.GetInstance();
    }

    public void MoveCard(Card card, int targetCol, int targetRow)
    {
        if (targetRow < 0 || targetRow >= gridManager.maxRows)
            targetRow = card.GetGridPosition().Item2;

        var (oldCol, oldRow) = card.GetGridPosition();

        if (gridManager.GetCardAt(targetCol, targetRow) == null)
        {
            gridManager.SetCardAt(oldCol, oldRow, null);
            gridManager.SetCardAt(targetCol, targetRow, card);
            card.SetGridPosition(targetCol, targetRow);
            Player.GetInstance().mergeCardHandler.TryMergeColumn(targetCol);
        }
        else
        {
            Card other = gridManager.GetCardAt(targetCol, targetRow);
            if (other.value == card.value)
            {
                int newValue = card.value * 2;
                card.SetValue(newValue);
                Destroy(other.gameObject);
                gridManager.SetCardAt(targetCol, targetRow, card);
                gridManager.SetCardAt(oldCol, oldRow, null);
                card.SetGridPosition(targetCol, targetRow);
                Player.GetInstance().mergeCardHandler.TryMergeColumn(targetCol);
            }
            else
            {
                card.SetGridPosition(oldCol, oldRow);
            }
        }
    }
}
