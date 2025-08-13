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
        SpawnNewRow();
        SpawnNewRow();
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

    private int GetMaxValueOnBoard()
    {
        int maxVal = 0;
        for (int c = 0; c < maxColumns; c++)
        {
            for (int r = 0; r < maxRows; r++)
            {
                if (grid[c, r] != null)
                    maxVal = Mathf.Max(maxVal, grid[c, r].GetValue());
            }
        }
        return maxVal;
    }
    #endregion

    #region SETTERS


    public void SetCardValueAt(int col, int row, int newValue)
    {
        grid[col, row].SetValue(newValue);
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

    public bool IsLastCardOfColumn(Card card)
    {
        (int col, int row) = card.GetGridPosition();
        Card lastCard = GetLastCardOfColumn(col);
        return lastCard == card;
    }

    public void SpawnNewRow(int debugValue = 0)
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
            int baseValue = grid[col, 1].GetValue();

            List<int> possibleValues = new List<int>();

            if (baseValue != 0)
            {
                int[] tempValues = { baseValue / 2, baseValue * 2, baseValue * 4 };

                foreach (int v in tempValues)
                {
                    if (v >= 2)
                        possibleValues.Add(v);
                }
            }
            else
            {
                possibleValues = new List<int>() { 2, 4 };
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
        }
    }
}
