using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    public GameObject cardPrefab;
    public RectTransform gridContainer;

    public int maxColumns = 4;
    public int maxRows = 8;

    private Vector2 cellSize = new Vector2(0, 0);
    private Vector2 spacing = new Vector2(0, 0);

    private Card[,] grid;
    int[] possibleValues = new int[] { 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        UpdateCellSize();
        UpdateCellSpacing();
    }

    public static GridManager GetInstance()
    {
        return Instance;
    }

    void Start()
    {
        ClearCards();
        InitializeCards();
        //SpawnNewRow();
        SpawnNewRow();
    }

    private void InitializeCards()
    {
        grid = new Card[maxColumns, maxRows];
        for (int i = 0; i < maxRows; i++)
        {
            for (int j = 0; j < maxColumns; j++)
            {
                GameObject gameObject = Instantiate(cardPrefab, gridContainer);
                Card card = gameObject.GetComponent<Card>();
                card.SetValue(0);
                card.SetGridPosition(j, i);
                card.gridManager = this;
                grid[j, i] = card;
            }
        }
    }

    private void ClearCards()
    {
        for (int i = gridContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(gridContainer.GetChild(i).gameObject);
        }
    }

    private void UpdateCellSize()
    {
        cellSize = gridContainer.gameObject.GetComponent<GridLayoutGroup>().cellSize;
    }

    private void UpdateCellSpacing()
    {
        spacing = gridContainer.gameObject.GetComponent<GridLayoutGroup>().spacing;
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

        // Convert from center origin to top-left origin
        float originX = localPoint.x + (gridContainer.rect.width / 2);

        float colFloat = originX / (cellSize.x + spacing.x);
        int col = Mathf.Clamp(Mathf.FloorToInt(colFloat), 0, maxColumns - 1);

        //Debug.Log($"[GetNearestColumn] Mouse localPosX: {localPoint.x:F2}, originX: {originX:F2}, colFloat: {colFloat:F2}, colIndex: {col}");

        return col;
    }

    public int GetNearestRow(Vector3 worldPos)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gridContainer, worldPos, null, out localPoint);

        // Convert from center origin to top-left origin
        float originY = -(localPoint.y - (gridContainer.rect.height / 2));

        float rowFloat = originY / (cellSize.y + spacing.y);
        int row = Mathf.Clamp(Mathf.FloorToInt(rowFloat), 0, maxRows - 1);

        //Debug.Log($"[GetNearestRow] Mouse localPosY: {localPoint.y:F2}, originY: {originY:F2}, rowFloat: {rowFloat:F2}, rowIndex: {row}");

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
                    grid[col, row + 1].SetValue(grid[col, row].GetValue());
                    grid[col, row].SetValue(0);
                }
            }
        }

        for (int col = 0; col < maxColumns; col++)
        {
            int val = 2;//possibleValues[Random.Range(0, 3)];
            grid[col, 0].SetValue(val);
        }
    }

    public int GetCardValueAt(int col, int row)
    {
        if (col < 0 || col >= maxColumns || row < 0 || row >= maxRows) return 0;
        return grid[col, row].GetValue();
    }

    public void SetCardValueAt(int col, int row, int newValue)
    {
        grid[col, row].SetValue(newValue);
    }

    public Card GetLastCardOfColumn(int col)
    {
        for (int row = maxRows - 1; row >= 0; row--)
        {
            if (grid[col, row] != null && grid[col, row].GetValue() != 0)
            {
                return grid[col, row];
            }
        }
        return null;
    }

    public bool IsLastCardOfColumn(Card card)
    {
        (int col, int row) = card.GetGridPosition();
        Card lastCard = GetLastCardOfColumn(col);
        return lastCard == card;
    }

}
