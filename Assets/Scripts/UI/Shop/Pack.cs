using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopableItem
{
    public E_ShopableItemType type = E_ShopableItemType.COIN;
    public int quantity = 1;
}

public class Pack : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private List<ShopableItem> items = new List<ShopableItem>();

    [Header("UI References")]
    public Button buttonBuy;

    private void Awake()
    {
        buttonBuy.onClick.AddListener(OnClickButtonBuy);
    }

    private void OnClickButtonBuy()
    {
        foreach (ShopableItem item in items)
        {
            var type = item.type;
            int quantity = item.quantity;
            switch (type)
            {
                case E_ShopableItemType.COIN:
                    CoinManager.Instance.AddCoin(quantity);
                    break;
                case E_ShopableItemType.DELETE:
                    break;
                case E_ShopableItemType.SUFFLE:
                    break;
                default:
                    break;
            }
        }
    }
}
