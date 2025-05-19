using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] List<GameObject> List;
    [SerializeField] GameObject TrafficCone;
    [SerializeField] GameObject Barrier;
    [SerializeField] GameObject OilBarrer;

    void Start()
    {
        int a = 0;
        for (int i = 0; i < List.Count; i++)
        {
            a = UnityEngine.Random.Range();

            List[i].Add
        }
    }


}
