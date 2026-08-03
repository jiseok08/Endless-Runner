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
        State.Subscribe(Condition.RESET, ResetObstacle);
        State.Subscribe(Condition.FINISH, EndObstacle);
    }

    public void OnInteract()
    {
        ReturnToPool();
    }

    private void EndObstacle()
    {
        canMove = false;
    }

    private void ResetObstacle()
    {
        ReturnToPool();
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
        State.UnSubscribe(Condition.RESET, ResetObstacle);
        State.UnSubscribe(Condition.FINISH, EndObstacle);
    }
}
