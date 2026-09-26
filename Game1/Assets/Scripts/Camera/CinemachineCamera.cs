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
        GameEvents.Subscribe(Condition.RESET, CameraReset);
        GameEvents.Subscribe(Condition.FINISH, Observe);
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
        GameEvents.UnSubscribe(Condition.RESET, CameraReset);
        GameEvents.UnSubscribe(Condition.FINISH, Observe);
    }
}
