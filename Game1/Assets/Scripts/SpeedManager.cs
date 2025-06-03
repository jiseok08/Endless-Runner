using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class SpeedManager : Singleton<SpeedManager>
{
    [SerializeField] float speed = 30f;
    [SerializeField] float limitSpeed = 60;

    [SerializeField] float initializeSpeed;

    public float Speed { get { return speed; } }
    
    public float InitializeSpeed { get { return initializeSpeed; } }

    private void OnEnable()
    {
        initializeSpeed = speed;

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
            yield return new WaitForSeconds(5.33f);

            speed += 2.5f;            
        }
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.START, Excute);
        State.UnSubscribe(Condition.FINISH, Release);
    }
}
