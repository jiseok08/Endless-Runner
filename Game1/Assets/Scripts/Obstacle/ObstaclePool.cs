using System.Collections.Generic;
using UnityEngine;

public interface IObstacleProvider // 의존 역전 원칙
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

    private Dictionary<ObstacleType, Queue<GameObject>> pools = new Dictionary<ObstacleType, Queue<GameObject>>()
        {
            { ObstacleType.Normal, new Queue<GameObject> () },
            { ObstacleType.Long, new Queue<GameObject> () }
        };

    [SerializeField] int createCount;

    private void Start()
    {
        var c = ConfigManager.Instance.Config.obstacleManager;

        longObstacleName = c.longObstacleName;
        obstacleNames = c.obstacleNames;

        createCount = c.createCount;

        for (int i = 0; i < createCount; i++) // 풀 채우기
        {
            GameObject obstacle = CreateObstacle(ObstacleType.Normal);

            pools[ObstacleType.Normal].Enqueue(obstacle);
        }
    }

    public GameObject GetObstacle(ObstacleType type) // 장애물 타입에 맞는 장애물을 반환합니다.
    {
        Queue<GameObject> pool = pools[type];

        return pool.Count > 0 ? pool.Dequeue() : CreateObstacle(type);
    }

    private GameObject CreateObstacle(ObstacleType type) // 장애물 타입에 맞는 장애물을 생성합니다.
    {
        GameObject obstacle = null;

        switch (type)
        {
            case ObstacleType.Normal:
                obstacle = Instantiate(Resources.Load<GameObject>(obstacleNames[Random.Range(0, obstacleNames.Length)]), transform);

                obstacle.name = obstacle.name.Replace("(Clone)", "");
                break;

            case ObstacleType.Long:
                obstacle = Instantiate(Resources.Load<GameObject>(longObstacleName), transform);

                obstacle.name = obstacle.name.Replace("(Clone)", "");
                break;

            default:
                Debug.LogError("CreateObstacle 함수 (타입 에러)");
                break;
        }

        if (obstacle == null)
        {
            Debug.LogError("CreateObstacle 함수 (obstacle == null)");
        }

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
