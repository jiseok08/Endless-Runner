using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoadLine
{
    LEFT = -1,
    MIDDLE = 0,
    RIGHT = 1,
}

public class Runner : MonoBehaviour
{
    [SerializeField] RoadLine roadLine;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] float speed;
    [SerializeField] float positionX = 0;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {

    }
    private void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        Keyboard();
    }


    void Keyboard()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (roadLine != RoadLine.LEFT)
            {
                roadLine--;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (roadLine != RoadLine.RIGHT)
            {
                roadLine++;
            }
        }


    }

    private void Move()
    {
        rigidBody.position = new Vector3(positionX * (int)roadLine, 0, 0);
    }

    private void Lerp()
        {
            Vector3.Lerp()
        }
}
