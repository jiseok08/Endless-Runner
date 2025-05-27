using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] float speed = 30f;
    [SerializeField] float LimitSpeed = 60;

    static SpeedManager instance;

    public float Speed { get { return speed; } }

    public static SpeedManager Instance { get { return instance; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartCoroutine(SpeedUp());
    }


    void Update()
    {
        
    }

    private IEnumerator SpeedUp()
    {
        while (true)
        {
            if (Speed < LimitSpeed)
            {
                speed += 2.5f;
            }

            yield return new WaitForSeconds(5.0f);
        }
    }
}
