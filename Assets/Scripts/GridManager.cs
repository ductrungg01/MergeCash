using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    #region SINGLETON
    public static GridManager GetInstance()
    {
        return Instance;
    }
    #endregion

    #region MonoBehavior Funcs
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

    void Start()
    {
        ClearCards(); // Clear Debug card prepared in design
        InitializeCards();
        SpawnNewRow(true);
        SpawnNewRow(true);
    }
    #endregion

    #region UPDATING
    private void UpdateCellSize()
    {
        cellSize = gridContainer.gameObject.GetComponent<GridLayoutGroup>().cellSize;
    }

    private void UpdateCellSpacing()
    {
        spacing = gridContainer.gameObject.GetComponent<GridLayoutGroup>().spacing;
    }

    #endregion

    #region GETTERS
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

    public int GetCardValueAt(int col, int row)
    {
        if (col < 0 || col >= maxColumns || row < 0 || row >= maxRows) return 0;
        return grid[col, row].GetValue();
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

    public bool GetHasMoney(int col, int row)
    {
        if (grid[col, row].GetValue()  == 0) return false;
        return grid[col, row].GetHasMoney();
    }

    public int GetMaxRows() { return maxRows; }

    public bool HasEmptyColumn()
    {
        for (int i = 0; i < maxColumns; i++)
        {
            if (grid[i, 0].GetValue() == 0) return true;
        }
        return false;
    }

    public int GetMaximumDepthOfGrid()
    {
        int maxDepth = 0;
        for (int c = 0; c < maxColumns; c++)
        {
            int depth = 0;
            for (int r = 0; r < maxRows; r++)
            {
                if (grid[c, r].GetValue() == 0) break;
                depth++;
            }
            maxDepth = Math.Max(maxDepth, depth);
        }
        return maxDepth;
    }

    public int GetLastValue(int col)
    {
        for (int r = maxRows - 1; r >= 0; --r)
        {
            if (grid[col, r].GetValue() != 0) return grid[col, r].GetValue();
        }
        return 0;
    }

    public int GetDepthestValue()
    {
        int maxDepth = GetMaximumDepthOfGrid();
        for (int c = 0; c < maxColumns; ++c)
        {
            if (grid[c, maxDepth].GetValue() != 0) return grid[c, maxDepth].GetValue();
        }
        return 0;
    }

    public List<int> GetLastValues()
    {
        List<int> lastVals = new List<int>();
        for (int c = 0; c < maxColumns;c++)
        {
            int last = GetLastValue(c);
            if (last != 0)
            {
                lastVals.Add(last);
            }
        }
        return lastVals;
    }

    public int GetMaxValueOnBoard()
    {
        int maxValue = int.MinValue;

        for (int row = 0; row < maxRows; row++)
        {
            for (int col = 0; col < maxColumns; col++)
            {
                maxValue = Math.Max(maxValue, grid[col, row].GetValue());
            }
        }

        return maxValue;
    }

    #endregion

    #region SETTERS
    public void SetCardValueAt(int col, int row, int newValue)
    {
        if (row >= maxRows)
        {
            // Process Gameover!
            Debug.Log("Gameover!");
        } else
        {
            grid[col, row].SetValue(newValue);
        }
    }
    #endregion

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
                grid[j, i] = card;
            }
        }
    }

    public void SetHasMoney(int col, int row, bool newValue)
    {
        grid[col, row].SetHasMoney(newValue);
    }

    private void ClearCards()
    {
        for (int i = gridContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(gridContainer.GetChild(i).gameObject);
        }
    }

    public bool IsLastCardOfColumn(Card card)
    {
        (int col, int row) = card.GetGridPosition();
        Card lastCard = GetLastCardOfColumn(col);
        return lastCard == card;
    }

    public void SpawnNewRow(bool isStarting = false, int debugValue = 0)
    {
        for (int col = 0; col < maxColumns; col++)
        {
            for (int row = maxRows - 2; row >= 0; row--)
            {
                if (grid[col, row] != null)
                {
                    grid[col, row + 1].SetValue(grid[col, row].GetValue());
                    grid[col, row + 1].SetHasMoney(grid[col, row].GetHasMoney());
                    grid[col, row].SetValue(0);
                }
            }
        }

        for (int col = 0; col < maxColumns; col++)
        {
            int baseValue = grid[col, 1].GetValue();

            if (baseValue >= 1024) continue;

            int nextRowBaseValue = grid[col, 2].GetValue();

            List<int> possibleValues = new List<int>();

            if (isStarting)
            {
                possibleValues = new List<int>() { 2, 4, 8 };
            }
            else
            {
                if (baseValue != 0)
                {
                    int currentMax = GetMaxValueOnBoard();
                    int capValue = Mathf.Min(currentMax, 2048);

                    possibleValues.Add(baseValue); 
                    if (Random.value < 0.5f) possibleValues.Add(Mathf.Min(baseValue * 2, capValue));
                    if (Random.value < 0.2f && baseValue > 2) possibleValues.Add(baseValue / 2);

                    if (Random.value < 0.3f) possibleValues.Add(2);
                    if (Random.value < 0.2f) possibleValues.Add(4);
                }
                else // is empty column
                {
                    if (GetMaximumDepthOfGrid() > 5)
                    {
                        int depthestValue = GetDepthestValue();
                        possibleValues.Add(depthestValue);
                    }
                    else
                    {
                        possibleValues = GetLastValues();
                        int n = possibleValues.Count;
                        for (int i = 0; i < n; ++i)
                        {
                            if (possibleValues[i] > 2) possibleValues.Add(possibleValues[i] / 2);
                        }
                    }
                }
            }

            int val;
            if (debugValue != 0)
            {
                val = debugValue;
            }
            else
            {
                val = possibleValues[Random.Range(0, possibleValues.Count)];
            }

            grid[col, 0].SetValue(val);
            grid[col, 0].SetHasMoney(Random.value <= 0.35f); // Money rate: 35%
        }

        Player.GetInstance().mergeCardHandler.TryMergeColumnFromTop();
    }
}
