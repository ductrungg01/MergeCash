using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

[ExecuteAlways]
public class Card : MonoBehaviour
{
    private static int ID_Counter = 0;
    public int ID { get; private set; }

    [SerializeField] private string label = "2";
    public string Label => label;

    [SerializeField] private int money = 0;
    [SerializeField] private Image backgroundImage = null;
    [SerializeField, Range(0, 1)] private float rateHasMoney = 0.5f;
    [SerializeField] private int defaultMoneyValue = 10;

    private CanvasGroup canvasGroup;

    [SerializeField] private TMP_Text text;

    [SerializeField] private Image moneyIcon;

    private Column ownerColumn;
    private CardDragHandler draggableItem;

    private int point = 0;
    private int coin = 0;
    private bool isDragging;

    #region MonoBehavior funcs

    private void OnValidate()
    {
        SetLabel(label);

        money = 0;
        if (Random.value < rateHasMoney)
        {
            SetMoney(defaultMoneyValue);
        }

        UpdateCardVisibility();
    }

    private void Awake()
    {
        ID = ID_Counter++;
        canvasGroup = GetComponent<CanvasGroup>();
        draggableItem = GetComponent<CardDragHandler>();
        if (!backgroundImage) backgroundImage = GetComponent<Image>();
        UpdateText();

        money = 0;
        if (Random.value < rateHasMoney)
        {
            SetMoney(defaultMoneyValue);
        }

        UpdateCardVisibility();
    }

    void Start()
    {
        UpdateText();
        UpdateCardVisibility();
    }

    private void Update()
    {
        if (followTarget != null)
        {
            transform.position = followTarget.transform.position + offset;
        }
    }

    void OnDestroy()
    {
        transform.DOKill(); 
    }
    #endregion

    #region SETTERS
    public void SetLabel(string newValue)
    {
        label = newValue;
        if (text != null)
        {
            text.SetText(label);
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

    public void SetCardData(CardData cardData)
    {
        SetLabel(cardData.label);
        SetBackground(cardData.background);
        SetPoint(cardData.point);
        SetCoin(cardData.coin);
    }

    public void SetCardData(string cardDataLabel)
    {
        var data = CardDataManager.Instance.GetCardDataByLabel(cardDataLabel);
        SetCardData(data);
    }

    private void SetPoint(int point) { this.point = point; }
    private void SetCoin(int coin) { this.coin = coin; }

    public void SetBackground(Sprite background)
    {
        if (background != null)
        {
            backgroundImage.sprite = background;
        }
        else
        {
            var fallback = CardDataManager.Instance.GetBackgroundByLabel(label);
            if (fallback != null)
                backgroundImage.sprite = fallback;
        }
    }

    public void SetIsDragging(bool isDragging)
    {
        this.isDragging = isDragging;
        canvasGroup.alpha = isDragging ? 0.6f : 1f;
    }

    #endregion

    #region GETTERS
    public string GetLabel() { return label; }

    private void UpdateText()
    {
        if (text != null)
            text.text = label;
    }

    public int GetMoney() { return money; }

    public Column GetOwnerColumn() { return ownerColumn; }

    public CardData GetCardData()
    {
        return new CardData(this.label, this.backgroundImage.sprite, this.coin, this.point);
    }

    #endregion

    void UpdateCardVisibility()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = (label.Length == 0 ? 0f : 1f);
        canvasGroup.interactable = label.Length != 0;
        canvasGroup.blocksRaycasts = label.Length != 0;

        if (!moneyIcon) Debug.LogError("Didn't setup money icon!");
        else
        {
            moneyIcon.gameObject.SetActive(money > 0);
        }
    }

    public void UpdateNextLabel()
    {
        var nextCardData = CardDataManager.Instance.GetNextCardData(label);
        if (nextCardData != null)
        {
            SetCardData(nextCardData);
        }
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
}
