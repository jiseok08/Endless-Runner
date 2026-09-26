using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
    private const string SaveKey = "Coin";

    [SerializeField] int coin;

    public int Coin => coin;

    protected override void Initialize()
    {
        coin = PlayerPrefs.GetInt(SaveKey, 0);  
    }

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.FINISH, Save);
    }

    public void AddCoin(int coinValue)
    {
        coin += coinValue;
    }

    public bool TrySpendCoin(int price)
    {
        if (coin >= price)
        {
            coin -= price;

            Save();

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
