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
        var player = Player.Instance;

        switch (type)
        {
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
}
