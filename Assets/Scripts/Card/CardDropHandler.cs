using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropHandler : MonoBehaviour, IDropHandler
{
    private Card card;

    private void Awake()
    {
        card = GetComponent<Card>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("On Drop in Card, ID:" + card.ID);

        if (eventData.pointerDrag == gameObject)
        {
            Debug.LogError("Drop on its card!");
            return;
        }
        
        if (card.GetOwnerColumn())
        {
            card.GetOwnerColumn().gameObject.GetComponent<ColumnDropHandler>().OnDrop(eventData);
        }
    }
}
