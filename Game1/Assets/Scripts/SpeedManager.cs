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

    protected override void Awake()
    {
        base.Awake();

        ConfigLoader.Load();

        var c = ConfigLoader.Config.SpeedManager;

        startSpeed = c.startSpeed;
        limitSpeed = c.limitSpeed;
        increaseSpeed = c.increaseSpeed;

        increaseTime = new WaitForSeconds(c.increaseTime);

        speed = startSpeed;
        initializeSpeed = startSpeed;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        speed = startSpeed;
        initializeSpeed = startSpeed;
    }

    private void OnEnable()
    {
        speed = startSpeed;
        initializeSpeed = startSpeed;

        SceneManager.sceneLoaded += OnSceneLoaded;

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

            speed += increaseSpeed;            
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        State.UnSubscribe(Condition.START, Excute);
        State.UnSubscribe(Condition.FINISH, Release);
    }
}
