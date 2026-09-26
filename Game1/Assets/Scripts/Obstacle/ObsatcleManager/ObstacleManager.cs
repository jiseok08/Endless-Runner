using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType
{
    Normal,
    Long
}

public interface ISpawnIntervalReceiver
{
    void DecreaseSpawnInterval(float amount);
}

public class ObstacleManager : MonoBehaviour, ISpawnIntervalReceiver
{
    private enum SpawnPattern
    {
        Single,
        Double,
        Triple,
        DoubleLong
    }

    private IObstacleProvider obstacleProvider;

    private ObstacleSpawner spawner;
    private SafeLineFinder safeLineFinder;

    [SerializeField] Transform[] spawnTransforms;

    [SerializeField] float startCycle;
    [SerializeField] float startMinCycle;
    [SerializeField] float cycleDecrease;

    [SerializeField] int tripleProbability;
    [SerializeField] int doubleLongProbability;
    [SerializeField] int standardStageCount;

    [SerializeField] float cycle;
    [SerializeField] float minCycle;

    int plannedCount; // 패턴 결정용
    int spawnedCount; // 생성 주기 감소용
    int lookAheadCount;

    private Coroutine spawnCoroutine;

    private Dictionary<SpawnPattern, ISpawnStrategy> spawnStrategies;

    private void Awake()
    {
        safeLineFinder = GetComponent<SafeLineFinder>();

        obstacleProvider = GetComponent<IObstacleProvider>();

        spawner = new ObstacleSpawner(obstacleProvider);

        spawnStrategies = new Dictionary<SpawnPattern, ISpawnStrategy>
        {
            { SpawnPattern.Single, new SingleSpawnStrategy() },
            { SpawnPattern.Double, new DoubleSpawnStrategy() },
            { SpawnPattern.Triple, new TripleSpawnStrategy() },
            { SpawnPattern.DoubleLong, new DoubleLongSpawnStrategy() }
        };
    }

    private void Start()
    {
        var c = ConfigManager.Instance.Config.obstacleManager;

        startCycle = c.startCycle;
        startMinCycle = c.startMinCycle;
        cycleDecrease = c.cycleDecrease;

        tripleProbability = c.tripleProbability;
        doubleLongProbability = c.doubleLongProbability;
        standardStageCount = c.standardStageCount;
        lookAheadCount = c.lookAheadCount;

        cycle = startCycle;
        minCycle = startMinCycle;

        ItemManager.Instance.Registry.Register<ISpawnIntervalReceiver>(this);
    }

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.START, StartSpawning);
        GameEvents.Subscribe(Condition.FINISH, Release);
        GameEvents.Subscribe(Condition.RESET, ResetSetting);
    }

    private void StartSpawning()
    {
        if (spawnCoroutine != null)
        {
            return;
        }

        for (int i = 0; i < lookAheadCount; i++)
        {
            AddNextPattern();
        }

        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void Spawn()
    {
        var (success, safeLine, obstacleType) = safeLineFinder.FindSafeLine(spawner.GetPatterns());

        Debug.Log($"생성 검사: {success}, 선택 라인: {safeLine}, 장애물: {obstacleType}");

        if (!success)
        {
            return;
        }

        spawner.Spawn(spawnTransforms);

        UpdateDifficulty();

        AddNextPattern();
    }

    private void AddNextPattern()
    {
        SpawnPlan[] currentPlans = spawner.GetPatterns();

        SpawnPlan[] plans = new SpawnPlan[currentPlans.Length + 1];

        for (int i = 0; i < currentPlans.Length; i++)
        {
            plans[i] = currentPlans[i];
        }

        int lastIndex = plans.Length - 1;

        while (true)
        {
            plans[lastIndex] = spawnStrategies[SelectSpawnPattern()].CreatePlan();

            bool success = safeLineFinder.FindSafeLine(plans, updateLine: false).success;

            if (success)
            {
                break;
            }
        }

        spawner.AddPattern(plans[lastIndex]);

        plannedCount++;
    }

    private void UpdateDifficulty()
    {
        spawnedCount++;

        if (spawnedCount % standardStageCount == 0)
        {
            cycle = Mathf.Max(0.5f, Mathf.Max(minCycle, cycle - cycleDecrease));
        }
    }

    private SpawnPattern SelectSpawnPattern()
    {
        int stage = plannedCount / standardStageCount;

        switch (stage)
        {
            case 0:
                return SpawnPattern.Single;

            case 1:
                return SpawnPattern.Double;

            case 2:
                return Random.Range(0, tripleProbability) == 0 ? SpawnPattern.Triple : SpawnPattern.Double;

            default:
                if (Random.Range(0, doubleLongProbability) == 0)
                {
                    return SpawnPattern.DoubleLong;
                }

                if (Random.Range(0, tripleProbability) == 0)
                {
                    return SpawnPattern.Triple;
                }

                return SpawnPattern.Double;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Spawn();

            yield return CoroutineCache.WaitForSeconds(cycle);
        }
    }

    private void ResetSetting()
    {
        Release();

        cycle = startCycle;
        minCycle = startMinCycle;

        plannedCount = 0;
        spawnedCount = 0;

        spawner.ClearPatterns();
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

    public void DecreaseSpawnInterval(float amount)
    {
        minCycle -= amount;
    }

    private void OnDisable()
    {
        Release();

        GameEvents.UnSubscribe(Condition.START, StartSpawning);
        GameEvents.UnSubscribe(Condition.FINISH, Release);
        GameEvents.UnSubscribe(Condition.RESET, ResetSetting);
    }
}