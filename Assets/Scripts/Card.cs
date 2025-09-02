using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteAlways]
public class Card : MonoBehaviour
{
    private static int ID_Counter = 0;
    public int ID { get; private set; }

    [SerializeField] private int value = 2;
    [SerializeField] private int money = 0;
    private CanvasGroup canvasGroup;

    [SerializeField] private TMP_Text text;

    [SerializeField] private Image moneyIcon;

    private Column ownerColumn;
    private CardDragHandler draggableItem;
    private bool isDragging = false;

    #region MonoBehavior funcs

    private void OnValidate()
    {
        SetValue(value);
        UpdateCardVisibility();
    }

    private void Awake()
    {
        ID = ID_Counter++;
        canvasGroup = GetComponent<CanvasGroup>();
        draggableItem = GetComponent<CardDragHandler>();
        UpdateText();
    }

    void Start()
    {
        UpdateText();
        UpdateCardVisibility();
    }

    private Card followTarget;
    private Vector3 offset;
    public void SetFollow(Card target)
    {
        followTarget = target;
        offset = transform.position - target.transform.position;
    }

    public void ClearFollow()
    {
        followTarget = null;
    }

    public static void ResetIDCounter()
    {
        ID_Counter = 0;
    }

    private void Update()
    {
        if (followTarget != null)
        {
            transform.position = followTarget.transform.position + offset;
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

    public void SetMoney(int newValue)
    {
        money = newValue;
        UpdateCardVisibility() ;
    }

    public void SetOwnerColumn(Column column)
    {
        this.ownerColumn = column;
        this.draggableItem.SetOwnerColumn(column);
    }

    public void SetIsDragging(bool isDragging)
    {
        this.isDragging = isDragging;
        canvasGroup.alpha = isDragging ? 0.6f : 1f;
    }

    #endregion

    #region GETTERS
    public int GetValue() { return value; }

    private void UpdateText()
    {
        if (text != null)
            text.text = value.ToString();
    }

    public int GetMoney() { return money; }

    public Column GetOwnerColumn() { return ownerColumn; }

    #endregion

    void UpdateCardVisibility()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = (value == 0 ? 0f : 1f);
        canvasGroup.interactable = value != 0;
        canvasGroup.blocksRaycasts = value != 0;

        if (!moneyIcon) Debug.LogError("Didn't setup money icon!");
        else
        {
            moneyIcon.gameObject.SetActive(money > 0);
        }
    }
}
