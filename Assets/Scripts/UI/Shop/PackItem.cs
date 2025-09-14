using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class PackItem : MonoBehaviour
{
    [Header("Data")]
    public Sprite icon;
    public string textValue;

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
