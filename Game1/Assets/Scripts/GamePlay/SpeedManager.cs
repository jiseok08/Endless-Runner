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
        GameEvents.Subscribe(Condition.RESET, ResetSpeed);
        GameEvents.Subscribe(Condition.START, Excute);
        GameEvents.Subscribe(Condition.FINISH, Release);
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
        GameEvents.UnSubscribe(Condition.RESET, ResetSpeed);
        GameEvents.UnSubscribe(Condition.START, Excute);
        GameEvents.UnSubscribe(Condition.FINISH, Release);
    }
}
