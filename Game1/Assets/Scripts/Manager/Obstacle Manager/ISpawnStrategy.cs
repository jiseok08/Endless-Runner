using UnityEngine;

public interface ISpawnStrategy
{
    void Spawn(IObstacleProvider obstacleProvider, Transform[] spawnPoints);
}
