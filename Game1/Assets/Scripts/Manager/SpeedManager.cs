using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedManager : Singleton<SpeedManager>
{
    [SerializeField] float speed;
    [SerializeField] float startSpeed;
    [SerializeField] float limitSpeed;
    [SerializeField] float increaseSpeed;


    [SerializeField] float initializeSpeed;

    [SerializeField] WaitForSeconds increaseTime;


    public float Speed { get { return speed; } }
    
    public float InitializeSpeed { get { return initializeSpeed; } }

    protected void Start()
    {
        var c = ConfigManager.Instance.Config.speedManager;

        startSpeed = c.startSpeed;
        limitSpeed = c.limitSpeed;
        increaseSpeed = c.increaseSpeed;

        increaseTime = new WaitForSeconds(c.increaseTime);

        ResetSpeed();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        ResetSpeed();
    }

    private void OnEnable()
    {
        State.Subscribe(Condition.RESET, ResetSpeed);
        State.Subscribe(Condition.START, Excute);
        State.Subscribe(Condition.FINISH, Release);
    }

    void Excute()
    {
        StartCoroutine(Increase());
    }

    void Release()
    {
        StopAllCoroutines();
    }

    private IEnumerator Increase()
    {
        while (Speed < limitSpeed)
        {
            yield return increaseTime;

            speed = Mathf.Min(speed + increaseSpeed, limitSpeed);
        }
    }

    private void ResetSpeed()
    {
        speed = startSpeed;
        initializeSpeed = startSpeed;
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.RESET, ResetSpeed);
        State.UnSubscribe(Condition.START, Excute);
        State.UnSubscribe(Condition.FINISH, Release);
    }
}
