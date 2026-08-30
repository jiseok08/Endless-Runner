using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
    [SerializeField] int coin = 9999999;

    public void AddCoin(int value)
    {
        coin += value;
    }

    public void DeductCoin(int value)
    {
        if (coin < 0)
        {
            coin = Mathf.Max(0, coin - value);
        }
    }

    public bool CompareCoin(int value)
    {
        if (coin >= value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
