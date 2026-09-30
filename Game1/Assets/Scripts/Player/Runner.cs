using UnityEngine;
using UnityEngine.InputSystem;

public class Runner : MonoBehaviour
{
    [SerializeField] RunnerMovement runnerMovement;
    [SerializeField] ShieldController shieldController;
    private PlayerInput playerInput;

    private void Awake()
    {
        runnerMovement = GetComponent<RunnerMovement>();
        shieldController = GetComponent<ShieldController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.RESET, ResetRunner);

        GameEvents.Subscribe(Condition.START, InputStart);

        GameEvents.Subscribe(Condition.FINISH, Release);
        GameEvents.Subscribe(Condition.FINISH, Die);
    }

    private void Start()
    {
        playerInput.DeactivateInput();
    }

    private void OnTriggerEnter(Collider other)
    {
        Obstacle obstacle = other.GetComponent<Obstacle>();

        if (obstacle == null)
        {
            return;
        }

        if (shieldController.TryShield())
        {
            obstacle.OnInteract();

            return;
        }

        GameEvents.Publish(Condition.FINISH);
    }

    public void InputStart()
    {
        playerInput.ActivateInput();
    }

    private void OnLeftMove(InputValue value)
    {
        if (value.isPressed)
        {
            runnerMovement.LeftMove();
        }
    }

    private void OnRightMove(InputValue value)
    {
        if (value.isPressed)
        {
            runnerMovement.RightMove();
        }
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            runnerMovement.TryJump();
        }
    }

    void Release()
    { 
        playerInput.DeactivateInput();

        runnerMovement.Release();
    }

    void ResetRunner()
    {
        runnerMovement.ResetMovement();
    }

    void Die()
    {
        AudioManager.Instance.PlayEffect("Conflict");
    }

    private void OnDisable()
    {
        GameEvents.UnSubscribe(Condition.RESET, ResetRunner);

        GameEvents.UnSubscribe(Condition.START, InputStart);

        GameEvents.UnSubscribe(Condition.FINISH, Release);
        GameEvents.UnSubscribe(Condition.FINISH, Die);
    }
}