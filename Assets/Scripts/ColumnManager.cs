using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

[ExecuteAlways]
public class ColumnManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float cardOffsetY = -120f;
    [SerializeField] private GameObject cardPrefab;

    private const int MAX_CARDS = 10;

    [Header("Data")]
    [SerializeField] private List<int> cardValues = new List<int>(); 
    [SerializeField] private List<GameObject> cards = new List<GameObject>();

    void OnValidate()
    {
        RefreshCardList();
        RearrangeColumn();
    }

    void Reset()
    {
        RefreshCardList();
    }

    public void RefreshCardList()
    {
        cards.Clear();
        foreach (Transform child in transform)
        {
            cards.Add(child.gameObject);
        }
    }

    public void AddCard(GameObject card)
    {
        if (cards.Count >= MAX_CARDS)
        {
            Debug.LogError("Reach the maximum card, cannot add more card!");
            return;
        }

        // set parent
        card.transform.SetParent(transform, false);
        cards.Add(card);

        RearrangeColumn();
    }

    public void RemoveCard(GameObject card)
    {
        if (cards.Contains(card))
        {
            cards.Remove(card);
            RearrangeColumn();
        }
    }

    public void RemoveCard(int index)
    {
        if (index < 0 || index >= cards.Count) return;

        GameObject card = cards[index];
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

    public int CardCount()
    {
        return cards.Count;
    }

    public GameObject GetCard(int index)
    {
        if (index < 0 || index >= cards.Count) return null;
        return cards[index];
    }

    public List<int> GetCardValues()
    {
        return cardValues;
    }
}
