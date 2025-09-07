using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CardData
{
    public string label;
    public Sprite background;

    public CardData(string label, Sprite background = null)
    {
        this.label = label;
        this.background = background;
    }
}
