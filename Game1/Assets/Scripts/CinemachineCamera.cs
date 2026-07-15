using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineCamera : MonoBehaviour
{
    [SerializeField] Runner runner;

    [SerializeField] CinemachineVirtualCamera aliveCamera;
    [SerializeField] CinemachineVirtualCamera deathCamera;

    private void OnEnable()
    {
        State.Subscribe(Condition.RESET, CameraReset);
        State.Subscribe(Condition.FINISH, Observe);
    }

    void CameraReset()
    {
        deathCamera.Priority = 0;
    }

    void Observe()
    {
        deathCamera.Priority = 20;
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.RESET, CameraReset);
        State.UnSubscribe(Condition.FINISH, Observe);
    }
}
