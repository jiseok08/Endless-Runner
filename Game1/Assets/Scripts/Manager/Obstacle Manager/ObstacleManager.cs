using System.Collections;
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

    [SerializeField] float startCycle;
    [SerializeField] float minCycle;
    [SerializeField] float cycleDecrease;

    [SerializeField] int tripleProb;
    [SerializeField] int standardSec;

    float cycle;
    float nextTime;

    int stepCount;

    private Coroutine spawnCoroutine;

    private void Awake()
    {
        obstacleProvider = GetComponent<IObstacleProvider>();
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
        nextTime = Time.time + standardSec;
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

        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void ResetSetting()
    {
        cycle = startCycle;
        nextTime = Time.time + standardSec;
        stepCount = 0;
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

    private void UpdateDifficulty()
    {
        if (Time.time >= nextTime)
        {
            cycle = Mathf.Max(minCycle, cycle - cycleDecrease); // 장애물 생성 주기 감소 (감소한 값이 최소값 이하일때는 최소값으로 변환)

            nextTime += standardSec;

            stepCount++;
        }
    }

    private void SpawnObstacle(int positionIndex, ObstacleType obstacleType)
    {
        GameObject obstacle = obstacleProvider.GetObstacle(obstacleType);

        if (obstacle == null)
        {
            Debug.LogError("SpawnObstacle 함수 (obstacle == null)");
        }

        obstacle.transform.position = spawnTransforms[positionIndex].position;

        obstacle.SetActive(true);
    }

    private IEnumerator SpawnRoutine() 
    {
        while(true) 
        {
            int positionIndex = Random.Range(0, spawnTransforms.Length); 

            UpdateDifficulty();

            switch (SelectSpawnPattern())
            {
                case SpawnPattern.Single:
                    SpawnObstacle(positionIndex, ObstacleType.Normal);
                    break;

                case SpawnPattern.Double:
                    SpawnObstacle(positionIndex, ObstacleType.Normal);
                    SpawnObstacle((positionIndex + 1) % spawnTransforms.Length, ObstacleType.Normal);
                    break;

                case SpawnPattern.Triple:
                    SpawnObstacle(positionIndex, ObstacleType.Normal);
                    SpawnObstacle((positionIndex + 1) % spawnTransforms.Length, ObstacleType.Normal);
                    SpawnObstacle((positionIndex + 2) % spawnTransforms.Length, ObstacleType.Long);
                    break;
            }

            yield return CoroutineCache.WaitForSeconds(cycle);
        }
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
