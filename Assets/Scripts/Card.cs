using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int value = 1;
    public TMP_Text text;

    private Vector2 startPos;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private int col, row;
    private Vector3 originalPosition;

    public GridManager gridManager;

    public void SetValue(int newValue)
    {
        value = newValue;
        if (text != null)
        {
            text.SetText(value.ToString());
        }
    }

    public void SetGridPosition(int c, int r)
    {
        col = c;
        row = r;
        UpdatePositionInUI();
    }

    public (int, int) GetGridPosition()
    {
        return (col, row);
    }

    public void UpdatePositionInUI()
    {
        Vector2 cellPos = gridManager.GetCellPosition(col, row);
        rectTransform.anchoredPosition = cellPos;
        originalPosition = rectTransform.position;
    }

    private void UpdateText()
    {
        if (text != null)
            text.text = value.ToString();
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        UpdateText();
    }

    //Ham debug object co dang duoc keo tha:
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Started dragging");
        startPos = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        originalPosition = rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging...");
        rectTransform.position = eventData.position;

        //transform.position = eventData.position;
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Stopped dragging");

        canvasGroup.blocksRaycasts = true;

        int targetCol = GridManager.GetInstance().GetNearestColumn(eventData.position);
        int targetRow = GridManager.GetInstance().GetNearestRow(eventData.position);

        Player.GetInstance().moveCardHandler.MoveCard(this, targetCol, targetRow);
    }



    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Left mouse clicked.");
        }

    }
}
