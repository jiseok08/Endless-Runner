using System.Collections;
using UnityEngine;
// 점프를 레이케스트로 체크하고 점프 모션을 동기화
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

    [SerializeField] Transform rayPoint;
    [SerializeField] float groundCheckDistance = 0.2f;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] float positionX;
    [SerializeField] float jumpPower;

    WaitForSeconds jumpCooldown;  

    private bool isJumping = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

        var c = ConfigManager.Instance.Config.runner;

        positionX = c.positionX;
        jumpPower = c.jumpPower;
        jumpCooldown = new WaitForSeconds(c.jumpCooldown);
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
        State.Subscribe(Condition.RESET, ResetRunner);
        State.Subscribe(Condition.START, InputSystem);
        State.Subscribe(Condition.START, StateTransition);

        State.Subscribe(Condition.FINISH, Die);
        State.Subscribe(Condition.FINISH, Release);
    }

    public void InputSystem()
    {
        StartCoroutine(Coroutine());
    }

    void Release()
    {
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void ResetRunner()
    {
        StopAllCoroutines();

        roadLine = RoadLine.MIDDLE;
        isJumping = false;
        rigidBody.position = new Vector3(0f, rigidBody.position.y, rigidBody.position.z);

        animator.Play("Idle");
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(rayPoint.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    IEnumerator Jump()
    {
        isJumping = true;

        animator.Play("Jump");
        rigidBody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        yield return new WaitUntil(() => !IsGrounded());

        yield return new WaitUntil(IsGrounded);

        isJumping = false;
    }

    IEnumerator Coroutine()
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

            if (Input.GetKeyDown(KeyCode.UpArrow) && !isJumping && !IsGrounded())
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
        State.UnSubscribe(Condition.RESET, ResetRunner);
        State.UnSubscribe(Condition.FINISH, Die);
        State.UnSubscribe(Condition.FINISH, Release);

        State.UnSubscribe(Condition.START, InputSystem);
        State.UnSubscribe(Condition.START, StateTransition);
    }
}