using UnityEngine;

public class DoubleSpawnStrategy : ISpawnStrategy
{
    private const int SpawnCount = 2;

    public SpawnPlan CreatePlan()
    {
        SpawnPlan createPattern = new SpawnPlan();

        int random = Random.Range(0, RoadLineInfo.RoadCount);

        for (int i = 0; i < SpawnCount; i++)
        {
            createPattern.AddSpawnData(RoadLineInfo.FromIndex((i + random) % RoadLineInfo.RoadCount), ObstacleType.Normal);
        }

        return createPattern;
    }
}
