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

        if (obstacle != null && !shieldController.TrySheild())
        {
            State.Publish(Condition.FINISH);
        }
    }

    private void OnEnable()
    {
        State.Subscribe(Condition.RESET, ResetRunner);

        State.Subscribe(Condition.START, StartInput);

        State.Subscribe(Condition.FINISH, Die);
        State.Subscribe(Condition.FINISH, Release);
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
        State.UnSubscribe(Condition.RESET, ResetRunner);

        State.UnSubscribe(Condition.START, StartInput);

        State.UnSubscribe(Condition.FINISH, Die);
        State.UnSubscribe(Condition.FINISH, Release);
    }
}