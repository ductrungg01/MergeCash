using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ActionButton : MonoBehaviour
{
    [SerializeField] private E_ShopableItemType actionType;

    [SerializeField] private Image iconSprite;
    [SerializeField] private Sprite actionSprite;

    [SerializeField] private TMP_Text txtCost;
    [SerializeField] private float actionCost = 100f;

    [SerializeField] private TMP_Text txtRemain;
    [SerializeField] private int remain = 0;

    private Button button;

    private void OnValidate()
    {
        SetActionSprite(actionSprite);
        SetActionCost(actionCost);
        SetRemain(remain);
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickActionButton);
    }

    private void Update()
    {
        if (Player.Instance == null) return;

        switch (actionType)
        {
            case E_ShopableItemType.DELETE:
                SetRemain(Player.Instance.GetRemainDeleteAction());
                break;
            case E_ShopableItemType.SHUFFLE:
                SetRemain(Player.Instance.GetRemainShuffleAction());
                break;
            default:
                break;
        }
    }

    private void OnClickActionButton()
    {
        if (remain > 0)
        {
            SetRemain(remain - 1);
            ProcessAction(true);
        }
        else
        {
            if (CoinManager.Instance.CurrentCoin >= actionCost)
            {
                CoinManager.Instance.UseCoin(Mathf.RoundToInt(actionCost));
                ProcessAction(false);
            }
        }
    }

    private void ProcessAction(bool useActioinItem)
    {
        if (GridManager.Instance.IsGridEmpty()) return;

        switch (actionType)
        {
            case E_ShopableItemType.DELETE:
                Player.Instance.ProcessDelete(useActioinItem);
                break;
            case E_ShopableItemType.SHUFFLE:
                Player.Instance.ProcessShuffle(useActioinItem);
                break;
            default:
                break;
        }
    }

    public void SetActionSprite(Sprite actionSprite)
    {
        if (actionSprite == null) return;

        this.actionSprite = actionSprite;
        iconSprite.sprite = actionSprite;
    }

    private void UpdateVisibility()
    {
        txtRemain.gameObject.SetActive(remain > 0);
        txtCost.transform.parent.gameObject.SetActive(remain <= 0);
    }

    public void SetActionCost(float cost)
    {
        actionCost = cost;
        txtCost.text = cost.ToString();
        UpdateVisibility();
    }

    public void SetRemain(int remain)
    {
        this.remain = remain;
        txtRemain.text = remain.ToString();
        UpdateVisibility();
    }
}
