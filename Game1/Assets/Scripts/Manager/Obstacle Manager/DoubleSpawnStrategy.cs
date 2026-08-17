using UnityEngine;

public class DoubleSpawnStrategy : ISpawnStrategy
{
    private const int SpawnCount = 2;

    public void Spawn(IObstacleProvider obstacleProvider, Transform[] spawnPoints)
    {
        int positionIndex = Random.Range(0, spawnPoints.Length); 

        for (int i = 0; i < SpawnCount; i++)
        {
            GameObject obstacle = obstacleProvider.GetObstacle(ObstacleType.Normal);

            obstacle.transform.position = spawnPoints[(positionIndex + i) % spawnPoints.Length].position;

            obstacle.SetActive(true);
        }
    }
}
