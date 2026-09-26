using System.Collections;
using UnityEngine;

public class Runner : MonoBehaviour
{
    [SerializeField] RunnerMovement runnerMovement;
    [SerializeField] ShieldController shieldController;

    private void Awake()
    {
        runnerMovement = GetComponent<RunnerMovement>();
        shieldController = GetComponent<ShieldController>();
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

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.RESET, ResetRunner);

        GameEvents.Subscribe(Condition.START, StartInput);

        GameEvents.Subscribe(Condition.FINISH, Die);
        GameEvents.Subscribe(Condition.FINISH, Release);
    }

    public void StartInput()
    {
        StartCoroutine(InputRoutine());
    }

    void Release()
    {
        StopAllCoroutines();
    }

    void ResetRunner()
    {
        StopAllCoroutines();

        runnerMovement.ResetMovement();
    }

    IEnumerator InputRoutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                runnerMovement.LeftMove();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                runnerMovement.RightMove();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                runnerMovement.TryJump();
            }

            yield return null;
        }
    }

    void Die()
    {
        AudioManager.Instance.Listener("Conflict");
    }

    private void OnDisable()
    {
        GameEvents.UnSubscribe(Condition.RESET, ResetRunner);

        GameEvents.UnSubscribe(Condition.START, StartInput);

        GameEvents.UnSubscribe(Condition.FINISH, Die);
        GameEvents.UnSubscribe(Condition.FINISH, Release);
    }
}