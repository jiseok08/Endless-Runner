using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] List<GameObject> roads;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < roads.Count; i++)
        {
            roads[i].transform.Translate(speed * Vector3.back * Time.deltaTime);
        }
    }
}
