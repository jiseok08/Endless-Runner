using UnityEngine;

public class CoinRouteRunnerMovement : MonoBehaviour
{
    [SerializeField] RoadLine roadLine;
    [SerializeField] Rigidbody rigidBody;

    [SerializeField] LayerMask groundLayer;

    [SerializeField] float positionX;
    [SerializeField] float jumpPower;
    [SerializeField] float laneChangeTime;

    private bool isGrounded;
    private Vector3 initialPosition;

    public float JumpInitialSpeed
    {
        get
        {
            return jumpPower / rigidBody.mass;
        }
    }

    public void TryJump()
    {
        if (!isGrounded)
        {
            return;
        }

        isGrounded = false;

        rigidBody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    private void Move()
    {
        var pos = rigidBody.position;

        float targetX = positionX * (int)roadLine;

        Vector3 target = new Vector3(targetX, pos.y, pos.z);

        float moveSpeed = positionX / laneChangeTime;

        rigidBody.MovePosition(Vector3.MoveTowards(pos, target, moveSpeed * Time.fixedDeltaTime));
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        initialPosition = rigidBody.position;
    }

    private void Start()
    {
        var c = ConfigManager.Instance.Config.runner;

        positionX = c.positionX;
        jumpPower = c.jumpPower;
        laneChangeTime = c.laneChangeTime;
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
        }
    }

    public void RightMove()
    {
        if (roadLine != RoadLine.RIGHT)
        {
            roadLine++;
        }
    }

    public void ResetMovement()
    {
        roadLine = RoadLine.MIDDLE;
        isGrounded = false;

        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        rigidBody.position = new Vector3(0f, initialPosition.y, initialPosition.z);

        rigidBody.WakeUp();
    }

    private void OnCollisionStay(Collision collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) == 0)
        {
            return;
        }

        if (rigidBody.linearVelocity.y <= 0f)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) == 0)
        {
            return;
        }

        isGrounded = false;
    }

    private void OnDisable()
    {
        isGrounded = false;
    }
}