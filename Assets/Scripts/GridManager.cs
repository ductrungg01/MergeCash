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
    public Vector2 spacing = new Vector2(10, 10);

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
    }

    public static GridManager GetInstance()
    {
        return Instance;
    }

    void Start()
    {
        grid = new Card[maxColumns, maxRows];
        ClearDebugCards();
        SpawnNewRow();
        SpawnNewRow();
    }

    private void ClearDebugCards()
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
        return Mathf.Clamp(Mathf.RoundToInt(colFloat), 0, maxColumns - 1);
    }

    public int GetNearestRow(Vector3 worldPos)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(gridContainer, worldPos, null, out localPoint);

        float rowFloat = (-localPoint.y - cellSize.y / 2) / (cellSize.y + spacing.y);
        return Mathf.Clamp(Mathf.RoundToInt(rowFloat), 0, maxRows - 1);
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
            int val = possibleValues[Random.Range(0, 2)];
            CreateCardAtPosition(col, 0, val);
        }
    }

    public void CreateCardAtPosition(int col, int row, int value)
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

    public void SetCardAt(int col, int row, Card card)
    {
        grid[col, row] = card;
    }
}
