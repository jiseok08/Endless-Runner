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

    [SerializeField] WaitForSeconds jumpCooldown;  
    [SerializeField] float positionX;
    [SerializeField] float jumpPower;
    
    bool canJump = true;
    bool startReady = false;

    private IEnumerator Start()
    {
        while (SpeedManager.Instance == null)
        {
            yield return null;
        }

        ConfigLoader.Load();

        var c = ConfigLoader.Config.runner;
        positionX = c.positionX;
        jumpPower = c.jumpPower;
        jumpCooldown = new WaitForSeconds(c.jumpCooldown);

        startReady = true;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Obstacle obstacle = other.GetComponent<Obstacle>();

        if (obstacle != null)
        {
            State.Publish(Condition.FINISH);
        }
    }

    private void OnEnable()
    {
        State.Subscribe(Condition.START, InputSystem);
        State.Subscribe(Condition.START, StateTransition);

        State.Subscribe(Condition.FINISH, Die);
        State.Subscribe(Condition.FINISH, Release);
    }

    public void InputSystem()
    {
        StartCoroutine(Coroutin());
    }

    void Release()
    {
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        if (!startReady) return;
        Move();
    }


    IEnumerator Jump()
    {
        canJump = false;

        animator.Play("Jump");
        rigidBody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        yield return jumpCooldown;

        canJump = true;
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

            if (Input.GetKeyDown(KeyCode.UpArrow) && canJump)
            {
                StartCoroutine(Jump());
            }

            yield return null;
        }
    }

    private void Move()
    {
        var pos = rigidBody.position;

        float targetX = positionX * (int)roadLine;

        Vector3 target = new Vector3(targetX, pos.y, pos.z);

        rigidBody.MovePosition(
            Vector3.Lerp(
                pos,
                target,
                SpeedManager.Instance.Speed * Time.fixedDeltaTime
            )
        );
    }


    void Die()
    {
        animator.Play("Die");
        AudioManager.Instance.Listener("Conflict");
    }

    public void StateTransition()
    {
        animator.SetTrigger("Start");
    }

    public void Synchronize()
    {
        animator.speed = SpeedManager.Instance.Speed / SpeedManager.Instance.InitializeSpeed;
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.FINISH, Die);
        State.UnSubscribe(Condition.FINISH, Release);

        State.UnSubscribe(Condition.START, InputSystem);
        State.UnSubscribe(Condition.START, StateTransition);
    }
}