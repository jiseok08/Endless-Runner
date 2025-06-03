using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [SerializeField] float offset = 40.0f;
    [SerializeField] List<GameObject> roads;

    public void OnEnable()
    {
        State.Subscribe(Condition.START, Excute);
    }

    void Excute()
    {
        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine()
    {
        while (true)
        {
            for (int i = 0; i < roads.Count; i++)
            {
                roads[i].transform.Translate(Vector3.back * SpeedManager.Instance.Speed * Time.deltaTime);
            }

            yield return null;
        }
    }

    public void InitializePosition()
    {
        GameObject newRoad = roads[0];

        roads.Remove(newRoad);

        float newZ = roads[roads.Count - 1].transform.position.z + offset;
    
        newRoad.transform.position = new Vector3(0, 0, newZ);

        roads.Add(newRoad);
    }

    public void OnDisable()
    {
        State.UnSubscribe(Condition.START, Excute);
    }
}
