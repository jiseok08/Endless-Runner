using System.Collections.Generic;
using UnityEngine;

public interface ISafeRouteReceiver
{
    void ReceiveSafeRoute(RoadLine line, ObstacleType? obstacle);
}

public class SafeLineFinder : MonoBehaviour
{
    private RoadLine nowLine = RoadLine.MIDDLE;

    [SerializeField] List<MonoBehaviour> receiverComponents = new();

    private readonly List<ISafeRouteReceiver> routeReceivers = new();

    private readonly int[] moves =
    {
        (int)RoadLine.LEFT,
        (int)RoadLine.MIDDLE,
        (int)RoadLine.RIGHT
    };

    private void Awake()
    {
        foreach (MonoBehaviour component in receiverComponents)
        {
            if (component is ISafeRouteReceiver receiver)
            {
                routeReceivers.Add(receiver);
            }
            else
            {
                Debug.LogError("ISafeRouteReceiver가 아님");
            }
        }
    }

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.RESET, ResetLine);
    }

    public void ResetLine()
    {
        nowLine = RoadLine.MIDDLE;
    }

    public (bool success, RoadLine safeLine, ObstacleType? obstacle) 
        FindSafeLine(SpawnPlan[] plans, bool updateLine = true)
    {
        for (int i = moves.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            int temp = moves[i];
            moves[i] = moves[randomIndex];
            moves[randomIndex] = temp;
        }

        RoadLine? safeLine = Dfs(plans, 0, nowLine);

        if (!safeLine.HasValue)
        {
            return (false, RoadLine.MIDDLE, null);
        }

        RoadLine selectedLine = safeLine.Value;
        ObstacleType? obstacle = plans[0].GetSpawnData(selectedLine);

        if (updateLine)
        {
            nowLine = selectedLine;

            foreach (ISafeRouteReceiver receiver in routeReceivers)
            {
                receiver.ReceiveSafeRoute(selectedLine, obstacle);
            }
        }

        return (true, selectedLine, obstacle);
    }

    private RoadLine? Dfs(SpawnPlan[] plans, int depth, RoadLine line)
    {
        if (depth == plans.Length)
        {
            return line;
        }

        SpawnPlan plan = plans[depth];

        foreach (int move in moves)
        {
            int index = RoadLineInfo.ToIndex(line) + move;  

            if (index < 0 || index >= RoadLineInfo.RoadCount)
            {
                continue;
            }

            RoadLine next = RoadLineInfo.FromIndex(index);

            if (plan.GetSpawnData(next) == ObstacleType.Long)
            {
                continue;
            }

            if (Dfs(plans, depth + 1, next).HasValue)
            {
                return next;
            }
        }

        return null;
    }

    private void OnDisable()
    {
        GameEvents.UnSubscribe(Condition.RESET, ResetLine);
    }
}