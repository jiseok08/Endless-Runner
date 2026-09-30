using System.Collections.Generic;
using UnityEngine;

public interface ICoinProvider
{
    GameObject GetCoin();
}

public interface ICoinReturner
{
    void ReturnToPool(GameObject coin);
}

public class CoinPool : MonoBehaviour, ICoinProvider, ICoinReturner
{
    private Queue<GameObject> pool = new Queue<GameObject>();

    [SerializeField] string coinName;
    private GameObject coinPrefab;

    int createCount;

    private void Start()
    {
        var c = ConfigManager.Instance.Config.coinPool;
        coinName = c.coinName;
        coinPrefab = Resources.Load<GameObject>(coinName);
        createCount = c.createCount;

        for (int i = 0; i < createCount; i++)
        {
            pool.Enqueue(CreateCoin());
        }

    }

    private GameObject CreateCoin()
    {
        GameObject coin = Instantiate(coinPrefab, transform);

        coin.SetActive(false);

        coin.GetComponent<Coin>().Initialize(this);

        return coin;
    }

    public GameObject GetCoin()
    {
        return pool.Count > 0 ? pool.Dequeue() : CreateCoin();
    }

    public void ReturnToPool(GameObject coin)
    {
        coin.SetActive(false);

        pool.Enqueue(coin);
    }
}


