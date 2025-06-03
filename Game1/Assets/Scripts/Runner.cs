using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoadLine
{
    LEFT = -1,
    MIDDLE = 0,
    RIGHT = 1,
}

public class Runner : MonoBehaviour
{
    [SerializeField] RoadLine roadLine;
    [SerializeField] Rigidbody rigidBody;

    [SerializeField] Animator animator;
    [SerializeField] float positionX = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        State.Subscribe(Condition.FINISH, Die);
        State.Subscribe(Condition.FINISH, Release);

        State.Subscribe(Condition.START, InputSystem);
        State.Subscribe(Condition.START, StateTransition);
    }

    void Release()
    {
        StopAllCoroutines();
    }

    public void InputSystem()
    {
        StartCoroutine(Coroutin());
    }

    private void FixedUpdate()
    {
        Move();
    }

    IEnumerator Coroutin()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (roadLine != RoadLine.LEFT)
                {
                    roadLine--;

                    animator.Play("Left Avoid");
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (roadLine != RoadLine.RIGHT)
                {
                    roadLine++;

                    animator.Play("Right Avoid");
                }
            }

            yield return null;
        }
    }

    private void Move()
    {
        rigidBody.position = Vector3.Lerp
            (
            rigidBody.position,
            new Vector3(positionX * (int)roadLine, 0, 0),
            SpeedManager.Instance.Speed * Time.deltaTime
            );              
    }

    void Die()
    {
        animator.Play("Die");
    }

    public void Synchronize()
    {
        animator.speed = SpeedManager.Instance.Speed / SpeedManager.Instance.InitializeSpeed;
    }

    public void StateTransition()
    {
        animator.SetTrigger("Start");
    }

    private void OnTriggerEnter(Collider other)
    {
        Obstacle obstacle = other.GetComponent<Obstacle>();

        if (obstacle != null)
        {
            State.Publish(Condition.FINISH);
        }
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.FINISH, Die);
        State.UnSubscribe(Condition.FINISH, Release);

        State.UnSubscribe(Condition.START, InputSystem);
        State.UnSubscribe(Condition.START, StateTransition);
    }
}
