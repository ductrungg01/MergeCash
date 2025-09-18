using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CardData
{
    public string label;
    public Sprite background;
    public int point = 1;
    public int coin = 1;

    public CardData(string label, Sprite background = null, int coin = 1, int point = 1)
    {
        this.label = label;
        this.background = background;
        this.coin = coin;
        this.point = point;
    }
}
