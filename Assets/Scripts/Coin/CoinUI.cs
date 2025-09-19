using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentCoinText;

    private void OnEnable()
    {
        CoinManager.Instance.OnCoinChanged += UpdateCointUI;
    }

    private void OnDisable()
    {
        CoinManager.Instance.OnCoinChanged -= UpdateCointUI;
    }

    private void Start()
    {
        UpdateCointUI();
    }

    private void UpdateCointUI()
    {
        if (currentCoinText != null) 
            currentCoinText.text = CoinManager.Instance.CurrentCoin.ToString();
    }
}
