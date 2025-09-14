using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

[ExecuteAlways]
public class ShopItem : MonoBehaviour
{
    [Header("Data")]
    public Sprite icon;
    public int quantity;
    public float price = 0f;
    public string currency = "$";

    [Header("UI References")]
    public Image iconImage;
    public TMP_Text quantityText;
    public TMP_Text priceText;

    private void OnValidate()
    {
        UpdateUI();
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
