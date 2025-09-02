using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class CardDataRow
{
    public List<CardData> row = new List<CardData>();
}

public class GridManager : MonoBehaviour
{
    [SerializeField] private RectTransform gridContainer;
    [SerializeField] private GameObject columnPrefab;

    [SerializeField] const int MAX_COLUMNS = 4;
    [SerializeField] const int MAX_ROWS = 8;


    [SerializeField] private bool useRandomData = false;

    [Tooltip("List of starting value of each column")]
    [SerializeField] List<CardDataRow> startingCardDatas = new List<CardDataRow> ();

    [SerializeField] private List<int> presetRandomData = new List<int>() { 2, 4, 8, 16, 32 };

    private List<Column> columns = new List<Column> ();


    public static GridManager Instance { get; private set; }
    public static GridManager GetInstance()
    {
        return Instance;
    }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        RemoveAllColumns();
        Card.ResetIDCounter();
        Column.ResetIDCounter();
        GenerateStartingColumn();
    }

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

        var startingDatas = useRandomData ? new List<CardDataRow>() : startingCardDatas;

        if (useRandomData)
        {
            for (int i = 0; i < MAX_COLUMNS; ++i)
            {
                List<CardData> datas = new List<CardData>();
                int rndLength = Random.Range(0, 4) + 1;
                while (rndLength > 0) 
                {
                    rndLength--;
                    int rndCardValue = presetRandomData[Random.Range(0, presetRandomData.Count)];
                    bool hasMoney = Random.value > 0.5f;
                    int cardMoney = (hasMoney ? 10 : 0);
                    datas.Add(new CardData(rndCardValue, cardMoney));
                }
                CardDataRow row = new CardDataRow();
                row.row = datas;
                startingDatas.Add(row);
            }
        }

        for (int i = 0; i < MAX_COLUMNS; ++i)
        {
            if (i >= startingDatas.Count) break;

            columns[i].SetCards(startingDatas[i].row);
        }
    }
}
