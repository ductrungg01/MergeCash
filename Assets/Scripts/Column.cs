using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Column : MonoBehaviour
{
    private static int ID_Counter = 0;

    public int ID { get; private set; }

    [Header("Settings")]
    [SerializeField] private float cardOffsetY = -120f;
    [SerializeField] private GameObject cardPrefab;

    private const int MAX_CARDS = 10;

    [Header("Data")]
    [SerializeField] private List<int> cardValues = new List<int>(); 
    [SerializeField] private List<Card> cards = new List<Card>();

    [Header("Debug")]
    [SerializeField]
    private List<CardData> debugCards = new List<CardData>()
    {
        new CardData("128"),
        new CardData("64"),
        new CardData("8"),
        new CardData("8")
    };

    void OnValidate()
    {
        RefreshCardList();
        RearrangeColumn();
    }

    private void Awake()
    {
        ID = ID_Counter++;
    }

    void Reset()
    {
        RefreshCardList();
    }

    public static void ResetIDCounter()
    {
        ID_Counter = 0;
    }

    public void RefreshCardList()
    {
        cards.Clear();
        foreach (Transform child in transform)
        {
            Card card = child.GetComponent<Card>();
            if (card != null)
            {
                cards.Add(card);
            }
        }
    }

    public void AddCard(Card card)
    {
        if (cards.Count >= MAX_CARDS)
        {
            Debug.LogError("Reach the maximum card, cannot add more card!");
            return;
        }

        // set parent
        card.gameObject.transform.SetParent(transform, false);
        card.SetOwnerColumn(this);
        cards.Add(card);

        RearrangeColumn();
    }

    public void RemoveCard(Card card)
    {
        if (cards.Contains(card))
        {
            card.SetOwnerColumn(null);
            cards.Remove(card);
            RearrangeColumn();
        }
    }

    public void RemoveCard(int index)
    {
        if (index < 0 || index >= cards.Count) return;

        var card = cards[index];
        card.SetOwnerColumn(null);
        cards.RemoveAt(index);
        cardValues.RemoveAt(index);

        Destroy(card);

        RearrangeColumn();
    }

    public void RearrangeColumn()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            RectTransform rt = cards[i].GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 1f);
            rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(0, i * cardOffsetY);

            // Need calculate card size dynamically later
            rt.sizeDelta = new Vector2(244f, 294f);
        }
    }

    private void ClearAllCards()
    {
        RefreshCardList();
        foreach (Card card in cards)
        {
            DestroyImmediate(card.gameObject);
        }
        cards.Clear();
        cardValues.Clear();
    }

    public void GenerateCardFromDebugCards()
    {
        SetCards(debugCards);
    }

    public void SetCards(List<CardData> cards)
    {
        ClearAllCards();
        foreach (var cardData in cards)
        {
            GameObject go = Instantiate(cardPrefab, transform);
            Card card = go.GetComponent<Card>();
            if (card != null)
            {
                card.SetCardData(cardData);
                card.SetOwnerColumn(this);

                AddCard(card);
            }
        }
    }

    public int CardCount()
    {
        return cards.Count;
    }

    public Card GetCard(int index)
    {
        if (index < 0 || index >= cards.Count) return null;
        return cards[index];
    }

    public List<int> GetCardValues()
    {
        return cardValues;
    }

    public void StartDragging(Card card)
    {
        int idx = cards.IndexOf(card);
        card.SetIsDragging(true);

        for (int i = idx + 1; i < cards.Count; i++)
        {
            cards[i].SetIsDragging(true);
            cards[i].SetFollow(card);
        }
    }

    public void StopDragging(Card card)
    {
        int idx = cards.IndexOf(card);
        card.SetIsDragging(false);

        for (int i = idx + 1; i < cards.Count; i++)
        {
            cards[i].SetIsDragging(false);
            cards[i].ClearFollow();
        }
    }

    public List<Card> GetCardsBelow(Card card)
    {
        int index = cards.IndexOf(card);
        if (index == -1) return new List<Card>();
        return cards.GetRange(index, cards.Count - index); 
    }
}
