using UnityEngine;

public class TripleSpawnStrategy : ISpawnStrategy
{
    private const int SpawnCount = 3;

    public SpawnPlan CreatePlan()
    {
        SpawnPlan createPattern = new SpawnPlan();

        int random = Random.Range(0, RoadLineInfo.RoadCount);

        for (int i = 0; i < SpawnCount; i++)
        {
            createPattern.AddSpawnData(RoadLineInfo.FromIndex((i + random) % RoadLineInfo.RoadCount), 
                i == SpawnCount - 1 ? (ObstacleType.Long) : (ObstacleType.Normal));
        }

        return createPattern;
    }
}


