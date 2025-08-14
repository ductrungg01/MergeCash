using UnityEngine;

public class MoveCardHandler : MonoBehaviour
{
    private GridManager gridManager;

    private void Start()
    {
        gridManager = GridManager.GetInstance();
    }

    public void MoveCard(Card card, int targetCol, bool isSpawnNewRowAfterMove = true)
    {
        Card targetCard = gridManager.GetLastCardOfColumn(targetCol);
        var (oriCol, oriRow) = card.GetGridPosition();

        bool mergeAnyHasMoneyCard = false;
        if (targetCard != null)
        {
            if (targetCard.GetValue() == card.GetValue())
            {
                if (card.GetHasMoney() || targetCard.GetHasMoney()) mergeAnyHasMoneyCard = true;

                int targetRow = targetCard.GetGridPosition().Item2;
                gridManager.SetCardValueAt(targetCol, targetRow, card.GetValue() * 2);
                if (Player.GetInstance().mergeCardHandler.TryMergeColumn(targetCol, ref mergeAnyHasMoneyCard))
                {
                    // Do not swpan new row if merge column success
                    isSpawnNewRowAfterMove = false;
                }
            } else
            {
                gridManager.SetCardValueAt(targetCol, targetCard.GetGridPosition().Item2 + 1, card.GetValue());
            }
            gridManager.SetCardValueAt(oriCol, oriRow, 0);
        } else
        {
            gridManager.SetCardValueAt(targetCol, 0, card.GetValue());
            gridManager.SetCardValueAt(oriCol, oriRow, 0);
        }

        // If merge any card which has money, we do not spawn new row!
        if (mergeAnyHasMoneyCard) isSpawnNewRowAfterMove = false;
        
        if (isSpawnNewRowAfterMove)
        {
            gridManager.SpawnNewRow();
        }
    }
}
