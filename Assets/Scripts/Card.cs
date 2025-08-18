using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[ExecuteAlways]
public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private int value = 2;
    [SerializeField] private bool hasMoney = false;
    public TMP_Text text;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private int col, row;

    private GridManager gridManager;
    [SerializeField] private Image moneyIcon;

    #region MonoBehavior funcs
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        gridManager = GridManager.GetInstance();
        UpdateText();
    }

    void Start()
    {
        UpdateText();
        UpdateCardVisibility();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Left mouse clicked.");
        }
    }
    #endregion

    #region SETTERS
    public void SetValue(int newValue)
    {
        value = newValue;
        if (text != null)
        {
            text.SetText(value.ToString());
        }

        UpdateCardVisibility();
    }

    public void SetHasMoney(bool newValue)
    {
        hasMoney = newValue;
        UpdateCardVisibility() ;
    }

    public void SetGridPosition(int c, int r)
    {
        col = c;
        row = r;
    }
    #endregion

    #region GETTERS
    public int GetValue() { return value; }

    public (int, int) GetGridPosition()
    {
        return (col, row);
    }

    private void UpdateText()
    {
        if (text != null)
            text.text = value.ToString();
    }

    public bool GetHasMoney() { return hasMoney; }

    #endregion

    #region DRAGGING
    private int originalCol;
    private Vector3 originalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!gridManager.IsLastCardOfColumn(this))
        {
            Debug.Log("This is not last card of column => do not drag");
            eventData.pointerDrag = null; // Cancel drag
            return;
        }

        originalCol = col; // Save the original column
        originalPosition = rectTransform.localPosition; // Save the original position

        Debug.Log($"Start drag from column: {originalCol}");

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        int targetCol = GridManager.GetInstance().GetNearestColumn(eventData.position);

        if (targetCol != originalCol)
        {
            Card targetLastCard = gridManager.GetLastCardOfColumn(targetCol);
            Debug.Log($"Drop to {targetCol} (last card: {targetLastCard?.GetValue()})");

            Player.GetInstance().moveCardHandler.MoveCard(this, targetCol);
        }
        else
        {
            Debug.Log("Same column => don't do anything");
        }

        rectTransform.localPosition = originalPosition;
        UpdateCardVisibility();
    }
    #endregion

    void UpdateCardVisibility()
    {
        canvasGroup.alpha = (value == 0 ? 0f : 1f);
        canvasGroup.interactable = value != 0;
        canvasGroup.blocksRaycasts = value != 0;

        if (!moneyIcon) Debug.LogError("Didn't setup money icon!");
        else
        {
            moneyIcon.gameObject.SetActive(hasMoney);
        }
    }
}
