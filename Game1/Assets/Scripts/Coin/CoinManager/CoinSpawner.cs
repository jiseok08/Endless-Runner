using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    private ICoinProvider coinProvider;

    [SerializeField] Transform coinRouteRunner;

    private WaitForSeconds spawnInterval;
    private float heightOffset;

    private Coroutine spawnCoroutine;

    private void Awake()
    {
        coinProvider = GetComponent<ICoinProvider>();
    }

    private void Start()
    {
        var c = ConfigManager.Instance.Config.coinSpawner;

        spawnInterval = CoroutineCache.WaitForSeconds(c.spawnInterval);
        heightOffset = c.heightOffset;
    }

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.START, StartSpawning);
        GameEvents.Subscribe(Condition.FINISH, StopSpawning);
    }

    private void StartSpawning()
    {
        if (spawnCoroutine != null)
        {
            return;
        }

        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnCoin();

            yield return spawnInterval;
        }
    }

    private void SpawnCoin()
    {
        GameObject coin = coinProvider.GetCoin();

        coin.transform.position = coinRouteRunner.position + Vector3.up * heightOffset;

        coin.SetActive(true);
    }

    private void StopSpawning()
    {
        if (spawnCoroutine == null)
        {
            return;
        }

        StopCoroutine(spawnCoroutine);

        spawnCoroutine = null;
    }

    private void OnDisable()
    {
        StopSpawning();

        GameEvents.UnSubscribe(Condition.START, StartSpawning);
        GameEvents.UnSubscribe(Condition.FINISH, StopSpawning);
    }
}