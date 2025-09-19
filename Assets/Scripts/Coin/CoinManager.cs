using UnityEngine;
using System;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int CurrentCoin { get; private set; }

    public event Action OnCoinChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        CurrentCoin = PlayerPrefs.GetInt("Coin", 0);
    }

    public void AddCoin(int amount)
    {
        CurrentCoin += amount;

        PlayerPrefs.SetInt("Coin", CurrentCoin);
        PlayerPrefs.Save();

        OnCoinChanged?.Invoke();
    }

    public void UseCoin(int amount)
    {
        CurrentCoin -= amount;

        PlayerPrefs.SetInt("Coin", CurrentCoin);
        PlayerPrefs.Save();

        OnCoinChanged?.Invoke();
    }

    public void ResetScore()
    {
        CurrentCoin = 0;
        OnCoinChanged?.Invoke();
    }
}
