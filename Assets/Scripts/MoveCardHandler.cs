using UnityEngine;

public class MoveCardHandler : MonoBehaviour
{
    private GridManager gridManager;

    private void Start()
    {
        gridManager = GridManager.GetInstance();
    }

    public void MoveCard(Card card, int targetCol)
    {
        Card targetCard = gridManager.GetLastCardOfColumn(targetCol);
        var (oriCol, oriRow) = card.GetGridPosition();

        if (targetCard != null)
        {
            if (targetCard.GetValue() == card.GetValue())
            {
                gridManager.SetCardValueAt(targetCol, targetCard.GetGridPosition().Item2, card.GetValue() * 2);
                gridManager.SetCardValueAt(oriCol, oriRow, 0);
            }
        } else
        {
            gridManager.SetCardValueAt(targetCol, 0, card.GetValue());
            gridManager.SetCardValueAt(oriCol, oriRow, 0);
        }
    }
}
