using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] List<GameObject> obstacles;

    [SerializeField] GameObject [ ] prefab;

    [SerializeField] int createCount = 5;

    void Start()
    {
        Create();
    }

    public void Create()
    {
        for (int i = 0; i < createCount; i++)
        {
            GameObject clone = Instantiate(prefab[Random.Range(0, prefab.Length)]);

            clone.SetActive(false);

            obstacles.Add(clone);
        }
    }
}
