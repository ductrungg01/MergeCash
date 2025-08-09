using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public RectTransform gridContainer;

    public int maxColumns = 4;
    public int maxRows = 8;

    public Vector2 cellSize = new Vector2(150, 200);
    public Vector2 spacing = new Vector2(10, 10);

    private Card[,] grid;

    int[] possibleValues = new int[] { 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };

    void Start()
    {
        grid = new Card[maxColumns, maxRows];
        SpawnNewRow();
    }

    public Vector2 GetCellPosition(int col, int row)
    {
        float x = (cellSize.x + spacing.x) * col + cellSize.x / 2;
        float y = -((cellSize.y + spacing.y) * row + cellSize.y / 2);
        return new Vector2(x, y);
    }

    public int GetNearestColumn(Vector3 worldPos)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gridContainer, worldPos, null, out localPoint);

        float colFloat = (localPoint.x - cellSize.x / 2) / (cellSize.x + spacing.x);
        int col = Mathf.Clamp(Mathf.RoundToInt(colFloat), 0, maxColumns - 1);
        return col;
    }
    public int GetNearestRow(Vector3 worldPos)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gridContainer, worldPos, null, out localPoint);

        float rowFloat = (-localPoint.y - cellSize.y / 2) / (cellSize.y + spacing.y);
        int row = Mathf.Clamp(Mathf.RoundToInt(rowFloat), 0, maxRows - 1);
        return row;
    }
    public void SpawnNewRow()
    {
        for (int col = 0; col < maxColumns; col++)
        {
            for (int row = maxRows - 2; row >= 0; row--)
            {
                if (grid[col, row] != null)
                {
                    grid[col, row + 1] = grid[col, row];
                    grid[col, row] = null;
                    grid[col, row + 1].SetGridPosition(col, row + 1);
                }
            }
        }

        for (int col = 0; col < maxColumns; col++)
        {
            int val = possibleValues[Random.Range(0, possibleValues.Length)];
            CreateCardAtPosition(col, 0, val);
        }
    }

    void CreateCardAtPosition(int col, int row, int value)
    {
        GameObject go = Instantiate(cardPrefab, gridContainer);
        Card card = go.GetComponent<Card>();
        card.gridManager = this;
        card.SetValue(value);
        card.SetGridPosition(col, row);

        grid[col, row] = card;
    }

    public Card GetCardAt(int col, int row)
    {
        if (col < 0 || col >= maxColumns || row < 0 || row >= maxRows) return null;
        return grid[col, row];
    }

    public void MoveCard(Card card, int targetCol, int targetRow)
    {
        if (targetRow < 0 || targetRow >= maxRows)
            targetRow = card.GetGridPosition().Item2;

        var (oldCol, oldRow) = card.GetGridPosition();

        if (grid[targetCol, targetRow] == null)
        {
            grid[oldCol, oldRow] = null;
            grid[targetCol, targetRow] = card;
            card.SetGridPosition(targetCol, targetRow);
            TryMergeColumn(targetCol);
        }
        else
        {
            Card other = grid[targetCol, targetRow];
            if (other.value == card.value)
            {
                int newValue = card.value * 2;
                card.SetValue(newValue);
                Destroy(other.gameObject);
                grid[targetCol, targetRow] = card;
                grid[oldCol, oldRow] = null;
                card.SetGridPosition(targetCol, targetRow);
                TryMergeColumn(targetCol);
            }
            else
            {
                card.SetGridPosition(oldCol, oldRow);
            }
        }
    }

    void TryMergeColumn(int col)
    {
        for (int row = 0; row < maxRows - 1; row++)
        {
            Card card1 = GetCardAt(col, row);
            Card card2 = GetCardAt(col, row + 1);
            if (card1 != null && card2 != null && card1.value == card2.value)
            {
                card1.SetValue(card1.value * 2);
                Destroy(card2.gameObject);
                ShiftColumnDown(col, row + 1);
                row--;
            }
        }
    }

    void ShiftColumnDown(int col, int startRow)
    {
        for (int row = startRow; row < maxRows - 1; row++)
        {
            grid[col, row] = grid[col, row + 1];
            if (grid[col, row] != null)
                grid[col, row].SetGridPosition(col, row);
        }
        grid[col, maxRows - 1] = null;
    }
}
