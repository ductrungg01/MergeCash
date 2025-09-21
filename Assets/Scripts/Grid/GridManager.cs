using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class StringListWrapper
{
    public List<string> cards = new List<string>();
}

public class GridManager : MonoBehaviour
{
    [SerializeField] private RectTransform gridContainer;
    [SerializeField] private GameObject columnPrefab;

    [SerializeField] const int MAX_COLUMNS = 4;
    [SerializeField] const int MAX_ROWS = 8;

    [SerializeField] private bool useRandomData = false;

    [Tooltip("List of starting value of each column")]
    [SerializeField]
    private List<StringListWrapper> startingCardDatas = new List<StringListWrapper>();

    [SerializeField] private List<int> presetRandomData = new List<int>() { 2, 4, 8, 16, 32, 64 };

    private List<Column> columns = new List<Column> ();


    #region Singleton
    public static GridManager Instance { get; private set; }
    public static GridManager GetInstance()
    {
        return Instance;
    }
    #endregion

    #region Monobehavior funcs
    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        UIManager.Instance.ShowLoading(true);
    }

    private void Start()
    {
        RemoveAllColumns();
        Card.ResetIDCounter();
        Column.ResetIDCounter();
        GenerateStartingColumn();
        UIManager.Instance.ShowLoading(false);
    }
    #endregion

    void RemoveAllColumns()
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }
    }

    void GenerateStartingColumn()
    {
        for (int i = 0; i < MAX_COLUMNS; ++i)
        {
            var columnGo = Instantiate(columnPrefab, gridContainer);
            columns.Add(columnGo.GetComponent<Column>());
        }

        var startingDatas = useRandomData ? new List<StringListWrapper>() : startingCardDatas;

        if (useRandomData)
        {
            for (int i = 0; i < MAX_COLUMNS; ++i)
            {
                List<string> datas = new List<string>();
                int rndLength = Random.Range(0, 4) + 1;
                while (rndLength > 0) 
                {
                    rndLength--;

                    while (true)
                    {
                        int rndCardValue = presetRandomData[Random.Range(0, presetRandomData.Count)];

                        if (datas.Count == 0)
                        {
                            datas.Add(rndCardValue.ToString());
                            break;
                        } else
                        {
                            string top = datas[datas.Count - 1];
                            if (rndCardValue.ToString() != top)
                            {
                                datas.Add(rndCardValue.ToString());
                                break;
                            }
                        }
                    }
                }
                var listString = new StringListWrapper();
                listString.cards = datas;
                startingDatas.Add(listString);
            }
        }

        for (int i = 0; i < MAX_COLUMNS; ++i)
        {
            if (i >= startingDatas.Count) break;

            columns[i].SetCards(startingDatas[i].cards);
        }
    }

    public void SpawnNewRow()
    {
        foreach (var column in columns)
        {
            column.AddCardFromTop(GetRandomCard(column));
        }
    }

    public void RemoveAllLastCard()
    {
        foreach (var column in columns)
        {
            if (column.CardCount()  > 0)
            {
                column.RemoveCard(column.CardCount() - 1, false);
            }
        }
    }

    public void ShuffleCards()
    {
        List<CardData> allCards = new List<CardData>();
        foreach (var column in columns)
        {
            allCards.AddRange(column.GetCardDatas());
        }

        System.Random rng = new System.Random();
        int n = allCards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (allCards[k], allCards[n]) = (allCards[n], allCards[k]);
        }

        int colCount = columns.Count;
        int baseSize = allCards.Count / colCount;  
        int remainder = allCards.Count % colCount;  

        int index = 0;
        for (int i = 0; i < colCount; i++)
        {
            int take = baseSize + (i < remainder ? 1 : 0);
            List<CardData> slice = allCards.GetRange(index, take);
            columns[i].SetCards(slice);
            index += take;
        }

        foreach (var column in columns)
        {
            StartCoroutine(column.TryMerge(false));
        }
    }

    #region Getters
    private const float SPAWN_RATIO_MAX = 0.3f; // 30% for max, 70% for min
    public CardData GetRandomCard(Column column)
    {
        var cardList = CardDataManager.Instance.GetCardList();
        if (cardList == null || cardList.Count == 0) return null;

        string maxLabel = GetMaxLabelOnBoard();
        string minLabel = GetMinLabelOnBoard();

        int maxIndex = cardList.FindIndex(c => c.label == maxLabel);
        int minIndex = cardList.FindIndex(c => c.label == minLabel);

        if (maxIndex < 0) maxIndex = 0;
        if (minIndex < 0) minIndex = 0;

        List<CardData> candidates = new List<CardData>();

        // Random: max or min
        if (Random.value < SPAWN_RATIO_MAX)
        {
            // ===== Spawn related to MAX =====
            int start = Mathf.Max(0, maxIndex - 4); // max/16
            int end = Mathf.Max(0, maxIndex - 1); // max/2

            for (int i = start; i <= end; i++)
            {
                var c = cardList[i];
                var topCard = column.GetCard(0);
                if (topCard == null || c.label != topCard.Label)
                    candidates.Add(c);
            }
        }
        else
        {
            // ===== Spawn related to MIN =====
            int start = Mathf.Max(0, minIndex - 2);
            int end = Mathf.Min(cardList.Count - 1, minIndex + 2); // [min, min * 4]

            for (int i = start; i <= end; i++)
            {
                var c = cardList[i];
                var topCard = column.GetCard(0);
                if (topCard == null || c.label != topCard.Label)
                    candidates.Add(c);
            }
        }

        if (candidates.Count == 0)
        {
            int fallbackIdx = Mathf.Clamp(maxIndex, 0, cardList.Count - 1);
            return cardList[fallbackIdx];
        }

        return candidates[Random.Range(0, candidates.Count)];
    }

    public bool ShouldSpawnNewRow(bool wasMerge)
    {
        Debug.Log($"[ShouldSpawnNewRow] WasMerge={wasMerge}");
        if (wasMerge)
        {
            int emptycolumn = CountEmptyColumn();
            int maxDepth = GetMaxDepth();

            Debug.Log($"[ShouldSpawnNewRow] MaxDepth={maxDepth}, MaxRows={MAX_ROWS}");
            if (emptycolumn == 1 && maxDepth <= MAX_ROWS / 2) return true;
            if (emptycolumn > 1 && maxDepth <= 2 * MAX_ROWS / 3) return true;
            return false;
        }

        return true;
    }

    public string GetMaxLabelOnBoard()
    {
        // Return the label with the highest rank across all columns
        string maxLabel = null;
        int maxIdx = -1;

        foreach (var column in columns) 
        {
            var maxCard = column.GetMaxCard();
            if (maxCard == null) continue;

            string label = maxCard.Label;
            int idx = CardDataManager.Instance.GetIndexByLabel(label);

            if (idx > maxIdx)
            {
                maxIdx = idx;
                maxLabel = label;
            }
        }

        return maxLabel;
    }

    public string GetMinLabelOnBoard()
    {
        string minLabel = null;
        int minIdx = int.MaxValue;

        foreach (var column in columns)
        {
            var minCard = column.GetMinCard();
            if (minCard == null) continue;

            int idx = CardDataManager.Instance.GetIndexByLabel(minCard.Label);
            if (idx >= 0 && idx < minIdx)
            {
                minIdx = idx;
                minLabel = minCard.Label;
            }
        }

        return minLabel;
    }

    public bool IsGridEmpty()
    {
        foreach (var column in columns)
        {
            if (column.CardCount() > 0) return false;
        }
        return true;
    }

    private int GetMaxDepth()
    {
        int maxDepth = 0;  
        foreach (var column in columns)
        {
            maxDepth = Mathf.Max(maxDepth, column.GetCards().Count);
        }
        return maxDepth;
    }

    private int CountEmptyColumn()
    {
        int count = 0;
        foreach(var column in columns)
        {
            if (column.GetCards().Count == 0) count++;
        }
        return count;
    }

    public bool IsGridOverCapacity()
    {
        foreach (var column in columns)
        {
            if (column.GetCards().Count > MAX_ROWS) return true;
        }

        return false;
    }
    #endregion
}
