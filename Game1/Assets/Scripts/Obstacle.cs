using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public void Activate()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.Translate(Vector3.up * SpeedManager.Instance.Speed *  Time.deltaTime);
    }
}
