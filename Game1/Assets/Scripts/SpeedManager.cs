using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : Singleton<SpeedManager>
{
    [SerializeField] float speed = 30f;
    [SerializeField] float limitSpeed = 60;

    public float Speed { get { return speed; } }

    void Start()
    {
        StartCoroutine(Increase());
    }

    private void OnEnable()
    {
        State.Subscribe(Condition.START, Excute);
    }

    void Excute()
    {
        StartCoroutine(Increase());
    }


    private IEnumerator Increase()
    {
        while (Speed < limitSpeed)
        {
            yield return new WaitForSeconds(5.0f);

            speed += 2.5f;            
        }
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.START, Excute);
    }
}
