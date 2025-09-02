using UnityEngine;

[System.Serializable]
public struct CardData
{
    public int value;
    public int money;

    public CardData(int value, int money)
    {
        this.value = value;
        this.money = money;
    }
}
