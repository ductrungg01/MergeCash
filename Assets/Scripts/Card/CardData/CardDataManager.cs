using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CardDataManager : MonoBehaviour
{
    private static CardDataManager _instance;
    public static CardDataManager Instance => _instance;

    private CardDataList cardDataList;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        cardDataList = Resources.Load<CardDataList>("CardDataList");
    }

    #region Getters
    public Sprite GetBackgroundByLabel(string label)
    {
        if (cardDataList == null) return null;
        var data = cardDataList.cards.FirstOrDefault(c => c.label == label);
        return data != null ? data.background : null;
    }

    public CardData GetNextCardData(string currentLabel)
    {
        if (cardDataList == null || cardDataList.cards == null || cardDataList.cards.Count == 0)
            return null;

        int index = cardDataList.cards.FindIndex(c => c.label == currentLabel);

        if (index >= 0 && index < cardDataList.cards.Count - 1)
        {
            return cardDataList.cards[index + 1];
        }

        return null;
    }

    public List<CardData> GetCardList()
    {
        return cardDataList != null ? cardDataList.cards : null;
    }

    public int GetIndexByLabel(string label)
    {
        // Return the index of the label in the configured CardDataList order
        if (cardDataList == null || cardDataList.cards == null) return -1;
        return cardDataList.cards.FindIndex(c => c.label == label);
    }
    #endregion
}
