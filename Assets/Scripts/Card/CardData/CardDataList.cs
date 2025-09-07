using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CardDataList", menuName = "Game/Card Data List")]
public class CardDataList : ScriptableObject
{
    public List<CardData> cards = new List<CardData>();
}
