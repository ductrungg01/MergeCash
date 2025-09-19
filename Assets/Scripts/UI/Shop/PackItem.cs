using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class PackItem : MonoBehaviour
{
    [Header("Data")]
    public Sprite icon;
    public string textValue;
    public E_ShopableItemType type = E_ShopableItemType.COIN;
    public int quantity = 1;

    [Header("UI References")]
    public Image iconImage;
    public TMP_Text valueText;

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

        if (valueText != null)
        {
            valueText.text = textValue;
        }
    }
}
