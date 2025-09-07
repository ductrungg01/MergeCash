using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColumnDropHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler,
    IPointerClickHandler, IPointerMoveHandler, IPointerUpHandler
{
    [SerializeField] private Image highlightImage;
    [SerializeField, Range(0, 1)] private float highlightAlpha = 0.5f;

    private Column column;

    private void Awake()
    {
        column = GetComponent<Column>();
        if (!highlightImage) highlightImage = GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("On Drop in Column, ID: " + column.ID);
        GameObject dropped = eventData.pointerDrag;
        CardDragHandler cardDragHandler = dropped.GetComponent<CardDragHandler>();
        if (cardDragHandler != null)
        {
            cardDragHandler.targetColumn = column;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CardDragHandler.IS_DRAGGING)
        {
            //Debug.Log("Pointer enter, Column ID: " + column.ID);
            Highlight();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CardDragHandler.IS_DRAGGING)
        {
            //Debug.Log("Pointer exit, Column ID: " + column.ID);
            Unhighlight();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CardDragHandler.IS_DRAGGING)
        {
            //Debug.Log("Pointer click, Column ID: " + column.ID);
            Highlight();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Pointer up, Column ID: " + column.ID);
        Unhighlight();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (CardDragHandler.IS_DRAGGING)
        {
            //Debug.Log("Pointer move, Column ID: " + column.ID);
        }
    }

    private void Highlight()
    {
        var color = highlightImage.color;
        highlightImage.color = new Color(color.r, color.g, color.b, highlightAlpha);
    }

    public void Unhighlight()
    {
        var color = highlightImage.color;
        highlightImage.color = new Color(color.r, color.g, color.b, 0);
    }
}
