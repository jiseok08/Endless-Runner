using System.Collections;
using UnityEngine;

public enum RoadLine
{
    LEFT = -1,
    MIDDLE = 0,
    RIGHT = 1
}

public static class RoadLineInfo
{ 
    public const int RoadCount = 3;

    public static int ToIndex(RoadLine roadLine)
    {
        return (int)roadLine + RoadCount / 2;
    }

    public static RoadLine FromIndex(int index)
    {
        return (RoadLine)(index - RoadCount / 2);
    }
}

public class RunnerMovement : MonoBehaviour
{
    [SerializeField] RoadLine targetLane;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Animator animator;

    [SerializeField] Transform rayPoint;
    [SerializeField] float groundCheckDistance; 
    [SerializeField] LayerMask groundLayer;

    [SerializeField] float positionX;
    [SerializeField] float jumpPower;
    [SerializeField] float laneChangeTime;

    [SerializeField] bool isJumping = false;

    float jumpHoldPoint = 0.4f;

    float animationSlow = 0.2f;

    private Vector3 StartPosition;

    public float JumpInitialSpeed => jumpPower / rigidBody.mass;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();

        StartPosition = rigidBody.position;
    }

    private void Start()
    {
        var c = ConfigManager.Instance.Config.runner;

        positionX = c.positionX;
        jumpPower = c.jumpPower;
        laneChangeTime = c.laneChangeTime;
    }

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.START, StateTransition);

        GameEvents.Subscribe(Condition.FINISH, Die);
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void LeftMove()
    {
        if (targetLane != RoadLine.LEFT)
        {
            targetLane--;

            if (isJumping == false)
            {
                animator.Play("Left Avoid");
            }
        }
    }

    public void RightMove()
    {
        if (targetLane != RoadLine.RIGHT)
        {
            targetLane++;

            if (isJumping == false)
            {
                animator.Play("Right Avoid");
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(rayPoint.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    public void TryJump()
    {
        if (isJumping || !IsGrounded())
        {
            return;
        }

        StartCoroutine(Jump());
    }

    private void Move()
    {
        var pos = rigidBody.position;

        float targetX = positionX * (int)targetLane;

        Vector3 target = new Vector3(targetX, pos.y, pos.z);

        float moveSpeed = positionX / laneChangeTime;

        rigidBody.MovePosition(Vector3.MoveTowards(pos, target, moveSpeed * Time.fixedDeltaTime));
    }

    IEnumerator Jump()
    {
        isJumping = true;

        animator.Play("Jump");

        rigidBody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        yield return new WaitUntil(() => // 체공 시작
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0); // 현재 진행중인 애니메이션 가져오기

            return state.IsName("Jump") && state.normalizedTime >= jumpHoldPoint; // 진행시간이 기준 시간 이상이라면 return
        });

        animator.speed = animationSlow; // 속도를 늦춰 착지 시간과 동기화

        yield return new WaitUntil(() => rigidBody.linearVelocity.y <= 0f && IsGrounded()); // 내려오는지 확인

        animator.speed = 1; // 속도 복구

        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            return !state.IsName("Jump");
        });

        isJumping = false;
    }

    public void ResetMovement()
    {
        Release();

        targetLane = RoadLine.MIDDLE;

        rigidBody.position = StartPosition;

        rigidBody.linearVelocity = Vector3.zero;

        animator.Play("Idle");
    }

    public void Release()
    {
        StopAllCoroutines();

        isJumping = false;
        animator.speed = 1f;
    }

    public void Synchronize()
    {
        animator.speed = SpeedManager.Instance.Speed / SpeedManager.Instance.InitializeSpeed;
    }

    public void StateTransition()
    {
        animator.SetTrigger("Start");
    }

    public void Die()
    {
        animator.Play("Die");
    }

    private void OnDisable()
    {
        GameEvents.UnSubscribe(Condition.START, StateTransition);

        GameEvents.UnSubscribe(Condition.FINISH, Die);
    }
}