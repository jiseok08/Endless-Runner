using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Normal,
    Long
}

public class ObstacleManager : MonoBehaviour
{
    private enum SpawnPattern
    {
        Single,
        Double,
        Triple
    }

    private IObstacleProvider obstacleProvider;

    [SerializeField] Transform[] spawnTransforms;

    // 기본 세팅값 캐싱
    [SerializeField] float startCycle;
    [SerializeField] float minCycle;
    [SerializeField] float cycleDecrease;

    [SerializeField] int tripleProb;
    [SerializeField] int standardSec;

    float cycle;
    float nextTime;

    int stepCount;

    private Coroutine spawnCoroutine;

    private Dictionary<SpawnPattern, ISpawnStrategy> spawnStrategies;

    private void Awake()
    {
        obstacleProvider = GetComponent<IObstacleProvider>();

        spawnStrategies = new Dictionary<SpawnPattern, ISpawnStrategy>
        {
            { SpawnPattern.Single, new SingleSpawnStrategy() },
            { SpawnPattern.Double, new DoubleSpawnStrategy() },
            { SpawnPattern.Triple, new TripleSpawnStrategy() }
        };
    }

    private void Start()
    {
        var c = ConfigManager.Instance.Config.obstacleManager;

        startCycle = c.startCycle;
        minCycle = c.minCycle;
        cycleDecrease = c.cycleDecrease;

        tripleProb = c.tripleProbability;
        standardSec = c.standardSecond;

        cycle = startCycle;
        stepCount = 0;
    }

    private void OnEnable()
    {
        State.Subscribe(Condition.START, StartSpawning);
        State.Subscribe(Condition.FINISH, Release);
        State.Subscribe(Condition.RESET, ResetSetting);
    }

    private void StartSpawning()
    {
        if (spawnCoroutine != null)
        {
            return;
        }

        nextTime = Time.time + standardSec;

        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void UpdateDifficulty()
    {
        if (Time.time >= nextTime)
        {
            cycle = Mathf.Max(minCycle, cycle - cycleDecrease); // 장애물 생성 주기 감소 (감소한 값이 최소값 이하일때는 최소값으로 변환)

            nextTime += standardSec;

            stepCount++;
        }
    }

    private SpawnPattern SelectSpawnPattern()
    {
        if (stepCount < 1)
        {
            return SpawnPattern.Single;
        }

        if (stepCount < 2)
        {
            return SpawnPattern.Double;
        }

        return Random.Range(0, tripleProb) == 0 ? SpawnPattern.Triple : SpawnPattern.Double;
    }

    private IEnumerator SpawnRoutine() 
    {
        while(true) 
        {
            UpdateDifficulty();

            spawnStrategies[SelectSpawnPattern()].Spawn(obstacleProvider, spawnTransforms); // 전략 패턴으로 생성

            yield return CoroutineCache.WaitForSeconds(cycle);
        }
    }

    private void ResetSetting()
    {
        cycle = startCycle;
        nextTime = Time.time + standardSec;
        stepCount = 0;
    }

    private void Release()
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
        State.UnSubscribe(Condition.START, StartSpawning);
        State.UnSubscribe(Condition.FINISH, Release);
        State.UnSubscribe(Condition.RESET, ResetSetting);
    }
}
