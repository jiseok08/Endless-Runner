using UnityEngine;

public class Obstacle : MonoBehaviour, ICollidable
{ 
    private ObstacleType type;
    private IObstacleReturner obstacleReturner;

    bool canMove;
    bool isInPool;

    public void Initialize(IObstacleReturner obstacleReturner, ObstacleType type)
    {
        this.obstacleReturner = obstacleReturner;
        this.type = type;
    }

    private void OnEnable()
    {
        canMove = true;
        isInPool = false;
        GameEvents.Subscribe(Condition.RESET, ReturnToPool);
        GameEvents.Subscribe(Condition.FINISH, EndObstacle);
    }

    public void OnInteract()
    {
        ReturnToPool();
    }

    private void EndObstacle()
    {
        canMove = false;
    }

    void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector3.up * SpeedManager.Instance.Speed * Time.deltaTime);
        }
    }

    private void ReturnToPool()
    {
        if (isInPool)
        {
            return;
        }

        isInPool = true;
        canMove = false;

        obstacleReturner.ReturnToPool(gameObject, type);
    }

    private void OnDisable()
    {
        GameEvents.UnSubscribe(Condition.RESET, ReturnToPool);
        GameEvents.UnSubscribe(Condition.FINISH, EndObstacle);
    }
}
