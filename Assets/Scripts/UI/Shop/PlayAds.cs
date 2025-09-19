using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayAds : MonoBehaviour
{
    [Header("Data")]
    public E_ShopableItemType type = E_ShopableItemType.COIN;
    public int quantity;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClickWatchAds);
    }

    private void OnClickWatchAds()
    {
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
