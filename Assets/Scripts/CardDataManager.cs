using UnityEngine;
using System.Linq;

public class CardDataManager : MonoBehaviour
{
    private static CardDataManager _instance;
    public static CardDataManager Instance => _instance;

    private CardDataList cardDataList;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        cardDataList = Resources.Load<CardDataList>("CardDataList");
    }
        
    public Sprite GetBackgroundByLabel(string label)
    {
        if (cardDataList == null) return null;
        var data = cardDataList.cards.FirstOrDefault(c => c.label == label);
        return data != null ? data.background : null;
    }
}
