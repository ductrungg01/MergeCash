using UnityEngine;

public class ColumnMergeCardHandler : MonoBehaviour
{
    private Column column;

    private void Awake()
    {
        column = GetComponent<Column>();
    }

    public bool CanMerge()
    {
        var cards = column.GetCards();
        for (int i = 0; i < cards.Count - 1; i++)
        {
            if (cards[i].Label == cards[i + 1].Label)
            {
                return true;
            }
        }
        return false;
    }

    private int GetMergeIndex()
    {
        var cards = column.GetCards();
        for (int i = 0; i < cards.Count - 1; i++)
        {
            if (cards[i].Label == cards[i + 1].Label)
            {
                return i + 1;
            }
        }

        return -1;
    }

    public bool TryMerge()
    {
        bool merged = false;

        while (CanMerge())
        {
            merged  = true;

            int index = GetMergeIndex();

            // Step 1. update next label of previous card
            Card prevCard = column.GetCard(index - 1);
            if (prevCard != null)
            {
                prevCard.UpdateNextLabel();
            }

            // Step 2. remove card and rearrange
            column.RemoveCard(index);
        }

        return merged;
    }
}
