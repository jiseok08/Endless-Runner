using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : Singleton<CoinManager>
{
    private const string SaveKey = "Coin";

    [SerializeField] Text[] coinText;

    [SerializeField] int coin;

    public int Coin => coin;

    public event Action CoinChanged;

    protected override void Initialize()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        coin = PlayerPrefs.GetInt(SaveKey, 0);

        CoinChanged?.Invoke();
    }

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.FINISH, Save);
    }

    public void AddCoin(int coinValue)
    {
        coin += coinValue;

        CoinChanged?.Invoke();
    }

    public bool TrySpendCoin(int price)
    {
        if (coin >= price)
        {
            coin -= price;

            Save();

            CoinChanged?.Invoke();

            return true;
        }
        else
        {
            return false;
        }
    }

    private void Save()
    {
        PlayerPrefs.SetInt(SaveKey, coin);
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        GameEvents.UnSubscribe(Condition.FINISH, Save);
    }
}
