using UnityEngine;

public class SingleSpawnStrategy : ISpawnStrategy
{
    public void Spawn(IObstacleProvider obstacleProvider, Transform[] spawnPoints)
    {
        int lane = Random.Range(0, spawnPoints.Length); ;

        GameObject obstacle = obstacleProvider.GetObstacle(ObstacleType.Normal);

        obstacle.transform.position = spawnPoints[lane].position;

        obstacle.SetActive(true);
    }
}
