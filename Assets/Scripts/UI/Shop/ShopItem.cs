using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System;

[ExecuteAlways]
public class ShopItem : MonoBehaviour
{
    [Header("Data")]
    public Sprite icon;
    public E_ShopableItemType type = E_ShopableItemType.COIN;
    public int quantity;
    public float price = 0f;
    public string currency = "$";

    [Header("UI References")]
    public Image iconImage;
    public TMP_Text quantityText;
    public TMP_Text priceText;
    public Button buttonBuy;

    private void OnValidate()
    {
        UpdateUI();
    }

    private void Awake()
    {
        buttonBuy.onClick.AddListener(OnClickButtonBuy);
    }

    private void OnClickButtonBuy()
    {
        var player = Player.Instance;

        switch (type) {
            case E_ShopableItemType.COIN:
                CoinManager.Instance.AddCoin(quantity);
                break;
            case E_ShopableItemType.DELETE:
                player.SetRemainDeleteAction(player.GetRemainDeleteAction() + quantity);
                break;
            case E_ShopableItemType.SHUFFLE:
                player.SetRemainShuffleAction(player.GetRemainShuffleAction() + quantity);
                break;
            default:
                break;
        }
    }

    private void UpdateUI()
    {
        if (iconImage != null)
        {
            iconImage.sprite = icon;
            iconImage.enabled = (icon != null);
        }

        quantityText.text = quantity.ToString();

        if (priceText != null)
        {
            priceText.text = price.ToString("F2", CultureInfo.InvariantCulture) + currency;
        }
    }
}
