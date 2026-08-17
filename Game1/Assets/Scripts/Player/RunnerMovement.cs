using System.Collections;
using UnityEngine;

public enum RoadLine
{
    LEFT = -1,
    MIDDLE = 0,
    RIGHT = 1
}

public class RunnerMovement : MonoBehaviour
{
    [SerializeField] RoadLine roadLine;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Animator animator;

    [SerializeField] Transform rayPoint;
    [SerializeField] float groundCheckDistance; 
    [SerializeField] LayerMask groundLayer;

    [SerializeField] float positionX;
    [SerializeField] float jumpPower;

    [SerializeField] bool isJumping = false;

    float jumpHoldPoint = 0.4f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        var c = ConfigManager.Instance.Config.runner;

        positionX = c.positionX;
        jumpPower = c.jumpPower;
    }

    private void OnEnable()
    {
        State.Subscribe(Condition.START, StateTransition);

        State.Subscribe(Condition.FINISH, Die);
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void LeftMove()
    {
        if (roadLine != RoadLine.LEFT)
        {
            roadLine--;

            if (isJumping == false)
            {
                animator.Play("Left Avoid");
            }
        }
    }

    public void RightMove()
    {
        if (roadLine != RoadLine.RIGHT)
        {
            roadLine++;

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
        if(isJumping || !IsGrounded())
        {
            return;
        }

        StartCoroutine(Jump());
    }

    private void Move()
    {
        var pos = rigidBody.position;

        float targetX = positionX * (int)roadLine;

        Vector3 target = new Vector3(targetX, pos.y, pos.z);

        rigidBody.MovePosition(Vector3.Lerp(pos, target, SpeedManager.Instance.Speed * Time.fixedDeltaTime));
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

        animator.speed = 0.2f; // 속도를 늦춰 착지 시간과 동기화

        yield return new WaitUntil(() => rigidBody.linearVelocity.y <= 0f && IsGrounded()); // 내려오는지 확인

        yield return new WaitUntil(IsGrounded); // 땅에 닿는지 확인

        animator.speed = 1; // 속도 복구

        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            return !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump");
        });

        isJumping = false;
    }

    public void ResetMovement()
    {
        roadLine = RoadLine.MIDDLE;
        isJumping = false;
        rigidBody.position = new Vector3(0f, rigidBody.position.y, rigidBody.position.z);


        animator.Play("Idle");
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

    public void Release()
    {
        StopAllCoroutines();

        isJumping = false;
        animator.speed = 1f;

        State.UnSubscribe(Condition.START, StateTransition);

        State.UnSubscribe(Condition.FINISH, Die);
    }
}