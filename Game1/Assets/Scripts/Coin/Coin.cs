using UnityEngine;

public class Coin : MonoBehaviour, ICollidable
{
    ICoinReturner coinReturner;

    private int coinValue;

    bool canMove;
    bool isInPool;

    private void Start()
    {
        var c = ConfigManager.Instance.Config.coin;

        coinValue = c.coinValue;
    }

    private void OnEnable()
    {
        canMove = true;
        isInPool = false;
        GameEvents.Subscribe(Condition.RESET, ReturnToPool);
        GameEvents.Subscribe(Condition.FINISH, EndCoin);
    }

    public void OnInteract()
    {
        ReturnToPool();
    }

    private void EndCoin()
    {
        canMove = false;
    }

    public void Initialize(ICoinReturner returner)
    {
        coinReturner = returner;
    }

    void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector3.back * SpeedManager.Instance.Speed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Runner runner = other.GetComponent<Runner>();

        if (runner != null)
        {
            CoinManager.Instance.AddCoin(coinValue);

            ReturnToPool();
        }
    }

    public void ReturnToPool()
    {
        if (isInPool)
        {
            return;
        }

        isInPool = true;
        canMove = false;

        coinReturner.ReturnToPool(gameObject);
    }

    private void OnDisable()
    {
        GameEvents.UnSubscribe(Condition.RESET, ReturnToPool);
        GameEvents.UnSubscribe(Condition.FINISH, EndCoin);
    }
}