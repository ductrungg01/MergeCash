using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Column ownerColumn;
    private Card card;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        card = GetComponent<Card>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public Column targetColumn = null;
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin Drag!");
        if (ownerColumn != null)
        {
            canvasGroup.blocksRaycasts = false;

            targetColumn = ownerColumn;
            ownerColumn.StartDragging(card);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("End Dragging");

        canvasGroup.blocksRaycasts = true;
        var sourceColumn = ownerColumn;

        List<Card> movingCards = ownerColumn.GetCardsBelow(card);

        sourceColumn.StopDragging(card);

        if (targetColumn != null)
        {
            if (targetColumn != ownerColumn)
            {
                foreach (var c in movingCards)
                {
                    sourceColumn.RemoveCard(c);
                    targetColumn.AddCard(c);
                }
            }
            else
            {
                ownerColumn.RearrangeColumn();
            }
        }
    }

    public void SetOwnerColumn(Column ownerColumn)
    {
        this.ownerColumn = ownerColumn;
    }
}
