using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CardDataManager : MonoBehaviour
{
    private static CardDataManager _instance;
    public static CardDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
#if UNITY_EDITOR
                _instance = FindFirstObjectByType<CardDataManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("CardDataManager");
                    _instance = go.AddComponent<CardDataManager>();
                }
#endif
            }
            return _instance;
        }
    }

    private CardDataList cardDataList;

    private void Awake()
    {
        Debug.Log("[CardDataManager] Awake called in " + (Application.isPlaying ? "Play Mode" : "Edit Mode"));
        _instance = this;

        cardDataList = Resources.Load<CardDataList>("CardDataList");

        if (cardDataList == null)
            Debug.LogError("CardDataList not found in Resources!");
        else
            Debug.Log("CardDataList loaded with " + cardDataList.cards.Count + " cards");
    }

    #region Getters
    public Sprite GetBackgroundByLabel(string label)
    {
        if (cardDataList == null) return null;
        var data = cardDataList.cards.FirstOrDefault(c => c.label == label);
        return data != null ? data.background : null;
    }

    public CardData GetCardDataByLabel(string label)
    {
        EnsureLoaded();

        if (cardDataList == null) return null;

        var data = cardDataList.cards.FirstOrDefault(c => c.label == label);
        if (data == null)
            Debug.LogWarning("No card found with label: " + label);
        return data;
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

    private void EnsureLoaded()
    {
        if (cardDataList == null)
        {
#if UNITY_EDITOR
            cardDataList = Resources.Load<CardDataList>("CardDataList");
            if (cardDataList == null)
                Debug.LogError("CardDataList.asset not found in Resources!");
#endif
        }
    }
}
