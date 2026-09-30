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

    [SerializeField] float startCycle;
    [SerializeField] float startMinCycle;
    [SerializeField] float limitCycle;
    [SerializeField] float cycleDecrease;

    [SerializeField] int standardStageCount;

    [SerializeField] float cycle;
    [SerializeField] float minCycle;

    private ObstacleStageConfig[] stages;

    int plannedCount; // 패턴 결정용
    int spawnedCount; // 생성 주기 감소용
    int lookAheadCount;

    private Vector3[] spawnPoints;

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
        limitCycle = c.limitCycle;
        cycleDecrease = c.cycleDecrease;

        standardStageCount = c.standardStageCount;
        lookAheadCount = c.lookAheadCount;

        stages = c.stages;

        spawnPoints = ConfigManager.Instance.Config.obstacleSpawn.spawnPoints;

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

            bool success = safeLineFinder.FindSafeLine(plans, false).success;

            if (success)
            {
                break;
            }
        }

        spawner.AddPattern(plans[lastIndex]);

        plannedCount++;
    }

    private void Spawn()
    {
        bool success = safeLineFinder.FindSafeLine(spawner.GetPatterns(), true).success;

        if (!success)
        {
            Debug.LogError("장애물 생성 가능한 경로가 없음");

            return;
        }

        spawner.Spawn(spawnPoints);

        UpdateDifficulty();

        AddNextPattern();
    }

    private SpawnPattern SelectSpawnPattern()
    {
        int stage = plannedCount / standardStageCount;

        stage = Mathf.Min(stage, stages.Length - 1);

        PatternWeightConfig[] patterns = stages[stage].patterns;

        int randomValue = Random.Range(0, 100);
        int accumulatedWeight = 0;

        for (int i = 0; i < patterns.Length; i++)
        {
            accumulatedWeight += patterns[i].weight;

            if (randomValue < accumulatedWeight)
            {
                return (SpawnPattern)System.Enum.Parse(typeof(SpawnPattern), patterns[i].pattern);
            }
        }

        return SpawnPattern.Single;
    }

    private void UpdateDifficulty()
    {
        spawnedCount++;

        if (spawnedCount % standardStageCount == 0)
        {
            cycle = Mathf.Max(limitCycle, Mathf.Max(minCycle, cycle - cycleDecrease));
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