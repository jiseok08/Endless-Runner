using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRouteRunner : MonoBehaviour, ISafeRouteReceiver
{
    private CoinRouteRunnerMovement runnerMovement;

    [SerializeField] Vector3[] obstacleSpawnPoints;

    private readonly Queue<(RoadLine line, ObstacleType? obstacle, float passDistance)> routes = new();

    private RoadLine nowLine = RoadLine.MIDDLE;

    private float jumpOffset;
    private float passMargin;

    private float movedDistance;

    private Coroutine runCoroutine;

    private void Awake()
    {
        runnerMovement = GetComponent<CoinRouteRunnerMovement>();
    }

    private void Start()
    {
        var c = ConfigManager.Instance.Config.coinRouteRunner;

        jumpOffset = c.jumpOffset;
        passMargin = c.passMargin;

        obstacleSpawnPoints = ConfigManager.Instance.Config.obstacleSpawn.spawnPoints;
    }

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.START, StartRunning);
        GameEvents.Subscribe(Condition.FINISH, StopRunning);
    }

    public void ReceiveSafeRoute(RoadLine line, ObstacleType? obstacle)
    {
        int index = RoadLineInfo.ToIndex(line);

        float distance = obstacleSpawnPoints[index].z - transform.position.z;

        float passDistance = movedDistance + distance;

        routes.Enqueue((line, obstacle, passDistance));
    }

    private void StartRunning()
    {
        if (runCoroutine != null)
        {
            return;
        }

        runCoroutine = StartCoroutine(RunRoutine());
    }

    private IEnumerator RunRoutine()
    {
        while (true)
        {
            yield return null;

            UpdateRoute();
        }
    }

    private void UpdateRoute()
    {
        float speed = SpeedManager.Instance.Speed;

        movedDistance += speed * Time.deltaTime;

        if (routes.Count == 0)
        {
            return;
        }

        var route = routes.Peek();

        float distance = route.passDistance - movedDistance;

        if (distance <= -passMargin)
        {
            routes.Dequeue();

            return;
        }

        MoveToLine(route.line);

        float jumpPeakTime = runnerMovement.JumpInitialSpeed / -Physics.gravity.y;

        float jumpDistance = speed * jumpPeakTime + jumpOffset;

        if (route.obstacle == ObstacleType.Normal && distance > 0f && distance <= jumpDistance)
        {
            runnerMovement.TryJump();
        }
    }

    private void MoveToLine(RoadLine line)
    {
        int direction = (int)line - (int)nowLine;

        if (direction < 0)
        {
            runnerMovement.LeftMove();

            nowLine--;
        }
        else if (direction > 0)
        {
            runnerMovement.RightMove();

            nowLine++;
        }
    }

    private void StopRunning()
    {
        if (runCoroutine != null)
        {
            StopCoroutine(runCoroutine);

            runCoroutine = null;
        }

        ResetRoute();
    }

    private void ResetRoute()
    {
        routes.Clear();

        nowLine = RoadLine.MIDDLE;

        movedDistance = 0f;

        runnerMovement.ResetMovement();
    }

    private void OnDisable()
    {
        StopRunning();

        GameEvents.UnSubscribe(Condition.START, StartRunning);
        GameEvents.UnSubscribe(Condition.FINISH, StopRunning);
    }
}