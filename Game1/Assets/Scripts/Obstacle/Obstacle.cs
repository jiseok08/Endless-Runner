using UnityEngine;

public class Obstacle : MonoBehaviour, Collidable
{
    bool canMove;

    private void OnEnable()
    {
        canMove = true;
        State.Subscribe(Condition.FINISH, ResetObstacle);
    }

    public void OnInteract()
    {
        gameObject.SetActive(false);
    }

    public void ResetObstacle()
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

    private void OnDisable()
    {
        State.UnSubscribe(Condition.FINISH, ResetObstacle);
    }
}
