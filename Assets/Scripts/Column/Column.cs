using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[ExecuteAlways]
public class Column : MonoBehaviour
{
    private static int ID_Counter = 0;

    public int ID { get; private set; }

    [Header("Settings")]
    [SerializeField] private float cardOffsetY = -120f;
    public float CardOffsetY => cardOffsetY;

    [SerializeField] private GameObject cardPrefab;

    private const int MAX_CARDS = 10;

    [Header("Data")]
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

    #region Monobehavior funcs
    void OnValidate()
    {
        RefreshCardList();
        RearrangeColumn();
    }

    private void Awake()
    {
        ID = ID_Counter++;
    }
    #endregion

    #region Setters
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

                AddCardFromBottom(card);
            }
        }
    }

    #endregion

    #region Getters
    public Card GetCard(int index)
    {
        if (index < 0 || index >= cards.Count) return null;
        return cards[index];
    }

    public List<Card> GetCards() { return cards; }

    public List<Card> GetCardsBelow(Card card)
    {
        int index = cards.IndexOf(card);
        if (index == -1) return new List<Card>();
        return cards.GetRange(index, cards.Count - index);
    }

    public Card GetMaxCard()
    {
        // Return the card that has the highest rank (by label index in CardDataList)
        if (cards == null || cards.Count == 0) return null;

        Card best = null;
        int bestIdx = -1;

        for (int i = 0; i < cards.Count; i++)
        {
            string label = cards[i].Label; 
            int idx = CardDataManager.Instance.GetIndexByLabel(label);
            if (idx > bestIdx)
            {
                bestIdx = idx;
                best = cards[i];
            }
        }

        return best;
    }

    public Card GetMinCard()
    {
        if (cards == null || cards.Count == 0) return null;

        Card best = null;
        int bestIdx = int.MaxValue;

        foreach (var card in cards)
        {
            int idx = CardDataManager.Instance.GetIndexByLabel(card.Label);
            if (idx >= 0 && idx < bestIdx)
            {
                bestIdx = idx;
                best = card;
            }
        }

        return best;
    }
    #endregion

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

    public void AddCardFromTop(CardData cardData)
    {
        GameObject go = Instantiate(cardPrefab, transform);
        Card card = go.GetComponent<Card>();
        if (card != null)
        {
            card.SetCardData(cardData);
            card.SetOwnerColumn(this);
        }

        cards.Insert(0, card);

        go.transform.SetAsFirstSibling();
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        SetupAnchor(rectTransform, new Vector2(0, -CardOffsetY));

        MoveAllCardDown(0.25f);
    }

    private void MoveAllCardDown(float duration)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            RectTransform rt = cards[i].GetComponent<RectTransform>();

            // Calculate the "correct" anchored position based on index
            Vector2 targetPos = new Vector2(0, i * cardOffsetY);

            // Animate from current position to target
            rt.DOAnchorPos(targetPos, duration).SetEase(Ease.OutQuad);
        }
    }

    public void AddCardFromBottom(Card card)
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

    public void RemoveCard(Card card, bool rearrangeAfterRemove = true)
    {
        if (cards.Contains(card))
        {
            card.SetOwnerColumn(null);
            cards.Remove(card);

            if (rearrangeAfterRemove)
            {
                RearrangeColumn();
            }
        }
    }

    public void RemoveCard(int index, bool rearrangeAfterRemove = true)
    {
        if (index < 0 || index >= cards.Count) return;

        var card = cards[index];
        card.SetOwnerColumn(null);
        cards.RemoveAt(index);

        Destroy(card.gameObject);

        if (rearrangeAfterRemove)
        {
            RearrangeColumn();
        }
    }

    private void SetupAnchor(RectTransform rt, Vector2 anchorPos)
    {
        rt.anchorMin = new Vector2(0.5f, 1f);
        rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
        rt.anchoredPosition = anchorPos;
    }

    public void RearrangeColumn()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            RectTransform rt = cards[i].GetComponent<RectTransform>();
            SetupAnchor(rt, new Vector2(0, i * cardOffsetY));

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
    }

    public void GenerateCardFromDebugCards()
    {
        SetCards(debugCards);
    }

    public int CardCount()
    {
        return cards.Count;
    }

    public IEnumerator TryMerge()
    {
        ColumnMergeCardHandler mergeHandler = GetComponent<ColumnMergeCardHandler>();
        if (mergeHandler != null)
        {
            yield return StartCoroutine(mergeHandler.TryMergeCoroutine());

            if (mergeHandler.WasMerged)
            {
                Debug.Log("Column had at least one merge");
            }
            else
            {
                Debug.Log("No merge happened");
            }

            if (GridManager.Instance.ShouldSpawnNewRow(mergeHandler.WasMerged))
            {
                GridManager.Instance.SpawnNewRow();
            }
           
        }
    }


    #region Dragging
    public void StartDragging(Card card)
    {
        int idx = cards.IndexOf(card);
        card.SetIsDragging(true);

        for (int i = idx + 1; i < cards.Count; i++)
        {
            cards[i].SetIsDragging(true);
            cards[i].SetFollow(card);
        }

        for (int i = idx;  i < cards.Count; i++)
        {
            cards[i].transform.SetParent(transform.root);
            cards[i].transform.SetAsLastSibling();
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

        for (int i = idx; i < cards.Count; i++)
        {
            cards[i].transform.SetParent(transform);
            cards[i].transform.SetAsLastSibling();
        }
    }
    #endregion
}
