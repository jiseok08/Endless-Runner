using System.Collections.Generic;
using UnityEngine;

public interface IObstacleProvider 
{
    GameObject GetObstacle(ObstacleType type);
}

public interface IObstacleReturner
{
    void ReturnToPool(GameObject obstacle, ObstacleType type);
}

public class ObstaclePool : MonoBehaviour, IObstacleProvider, IObstacleReturner
{
    [SerializeField] string longObstacleName;
    [SerializeField] string[] obstacleNames;

    private GameObject longObstaclePrefab;
    private GameObject[] obstaclePrefabs;

    private Dictionary<ObstacleType, Queue<GameObject>> pools = new Dictionary<ObstacleType, Queue<GameObject>>()
        {
            { ObstacleType.Normal, new Queue<GameObject> () },
            { ObstacleType.Long, new Queue<GameObject> () }
        };

    [SerializeField] int createCount;

    private void Start()
    {
        var c = ConfigManager.Instance.Config.obstaclePool;

        longObstacleName = c.longObstacleName;
        obstacleNames = c.obstacleNames;

        createCount = c.createCount;

        longObstaclePrefab = Resources.Load<GameObject>(longObstacleName);

        obstaclePrefabs = new GameObject[obstacleNames.Length];

        for (int i = 0; i < obstacleNames.Length; i++)
        {
            obstaclePrefabs[i] = Resources.Load<GameObject>(obstacleNames[i]);
        }

        for (int i = 0; i < createCount; i++)
        {
            GameObject obstacle = CreateObstacle(ObstacleType.Normal);

            pools[ObstacleType.Normal].Enqueue(obstacle);
        }
    }

    public GameObject GetObstacle(ObstacleType type)
    {
        Queue<GameObject> pool = pools[type];

        return pool.Count > 0 ? pool.Dequeue() : CreateObstacle(type);
    }

    private GameObject CreateObstacle(ObstacleType type)
    {
        GameObject obstacle = null;

        switch (type)
        {
            case ObstacleType.Normal:
                obstacle = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], transform);
                break;

            case ObstacleType.Long:
                obstacle = Instantiate(longObstaclePrefab, transform);
                break;

            default:
                Debug.LogError("CreateObstacle 함수 (타입 에러)");
                break;
        }

        obstacle.name = obstacle.name.Replace("(Clone)", "");

        obstacle.SetActive(false);

        obstacle.GetComponent<Obstacle>().Initialize(this, type);

        return obstacle;
    }

    public void ReturnToPool(GameObject obstacle, ObstacleType type)
    {
        obstacle.SetActive(false);

        pools[type].Enqueue(obstacle);
    }
}
