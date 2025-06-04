using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineCamera : MonoBehaviour
{
    [SerializeField] Runner runner;

    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;

    private void OnEnable()
    {
        State.Subscribe(Condition.FINISH, Follow);
        State.Subscribe(Condition.FINISH, Observe);
    }

    void Follow()
    {
        cinemachineVirtualCamera.Follow = runner.transform;
    }

    public void Observe()
    {
        cinemachineVirtualCamera.LookAt = runner.transform;
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.FINISH, Follow);
        State.UnSubscribe(Condition.FINISH, Observe);
    }
}
