using UnityEngine;

public class SingleSpawnStrategy : ISpawnStrategy
{
    public SpawnPlan CreatePlan()
    {
        SpawnPlan createPattern = new SpawnPlan();

        int random = Random.Range(0, RoadLineInfo.RoadCount);

        createPattern.AddSpawnData(RoadLineInfo.FromIndex(random), ObstacleType.Normal);

        return createPattern;
    }
}
