using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner
{
    private readonly Queue<SpawnPlan> patterns = new();   

    IObstacleProvider obstacleProvider;

    public ObstacleSpawner(IObstacleProvider obstacleProvider)
    {
        this.obstacleProvider = obstacleProvider;
    }

    public SpawnPlan[] GetPatterns()
    {
        return patterns.ToArray();
    }

    public void AddPattern(SpawnPlan newPattern)
    {
        patterns.Enqueue(newPattern);
    }

    public void Spawn(Vector3[] spawnPoints)
    {
        SpawnPlan createPattern = patterns.Dequeue();

        for (int i = 0; i < RoadLineInfo.RoadCount; i++)
        {
            RoadLine line = RoadLineInfo.FromIndex(i);

            ObstacleType? type = createPattern.GetSpawnData(line);  

            if (type == null)
            {
                continue;
            }

            GameObject obstacle = obstacleProvider.GetObstacle(type.Value);

            obstacle.transform.position = spawnPoints[i];

            obstacle.SetActive(true);
        }
    }

    public void ClearPatterns()
    {
        patterns.Clear();
    }
}
