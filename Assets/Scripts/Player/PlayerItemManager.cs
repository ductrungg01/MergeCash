using System.Collections.Generic;
using UnityEngine;

public class PlayerItemManager : MonoBehaviour
{
    private int remainDeleteAction = 0;
    private int remainShuffleAction = 0;

    private void Awake()
    {
        remainDeleteAction = PlayerPrefs.GetInt("Delete", 0);
        remainShuffleAction = PlayerPrefs.GetInt("Shuffle", 0);
    }

    public int GetRemainDeleteAction() {  return remainDeleteAction; }
    public int GetRemainShuffleAction() { return remainShuffleAction; }

    public void SetRemainDeleteAction(int newValue) { 
        remainDeleteAction = newValue;
        PlayerPrefs.SetInt("Delete", remainDeleteAction);
        PlayerPrefs.Save();
    }
    public void SetRemainShuffleAction(int newValue) { 
        remainShuffleAction = newValue;
        PlayerPrefs.SetInt("Shuffle", remainShuffleAction);
        PlayerPrefs.Save();
    }
}
