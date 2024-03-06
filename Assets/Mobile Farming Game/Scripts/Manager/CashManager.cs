using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashManager : MonoBehaviour
{
    public static CashManager instance;
    private int coin;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(instance.gameObject);
        loadData();
        updateCoinContainer();
    }
    public void addCoin(int _coin)
    {
        coin += _coin;
        updateCoinContainer();
        saveData();
    }
    public int getCurrentCoins() => coin;
    [NaughtyAttributes.Button]
    private void add500Coins()
    {
        coin += 500;
        updateCoinContainer();
        saveData();
    }
    private void loadData()
    {
        if (PlayerPrefs.HasKey("Coins"))
            coin = PlayerPrefs.GetInt("Coins");
        else coin = 1000;
    }
    private void saveData()
    {
        PlayerPrefs.SetInt("Coins",coin);
    }
    private void updateCoinContainer()
    {
        GameObject[] coinAmount = GameObject.FindGameObjectsWithTag("CoinAmount");
        foreach(var coinAmountItem in coinAmount)
        {
            coinAmountItem.GetComponent<TextMeshProUGUI>().text = coin.ToString();
        }
    }
}
