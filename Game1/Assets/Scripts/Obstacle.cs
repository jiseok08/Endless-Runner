using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    private void OnEnable()
    {
        State.Subscribe(Condition.FINISH, Release);
    }

    public void Activate()
    {
        gameObject.SetActive(false);
    }

    void Release()
    {
        Destroy(this);
    }

    void Update()
    {
        transform.Translate(Vector3.up * SpeedManager.Instance.Speed *  Time.deltaTime);
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.FINISH, Release);
    }
}
