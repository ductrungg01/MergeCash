using UnityEngine;
using UnityEngine.EventSystems;

public class ColumnDropHandler : MonoBehaviour, IDropHandler
{
    private Column column;

    private void Awake()
    {
        column = GetComponent<Column>();
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
}
