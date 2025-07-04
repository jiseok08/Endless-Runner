using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] int random;

    [SerializeField] int createCount = 5;

    [SerializeField] List<GameObject> obstacles;

    [SerializeField] string [ ] obstacleNames;

    [SerializeField] Transform [ ] transforms;

    private void OnEnable()
    {
        State.Subscribe(Condition.START, Excute);
        State.Subscribe(Condition.FINISH, Release);
    }

    void Awake()
    {
        obstacles.Capacity = 10;

        Create();
    }

    void Release()
    {
        StopAllCoroutines();
    }

    public void Create()
    {
        for (int i = 0; i < createCount; i++)
        {
            GameObject clone = Instantiate(Resources.Load<GameObject>(obstacleNames[Random.Range(0, obstacleNames.Length)]), transform);

            clone.name = clone.name.Replace("(Clone)", "");

            clone.SetActive(false);

            obstacles.Add(clone);
        }
    }

    bool ExmineActive()
    {
        for (int i = 0; i < obstacles.Count;i++)
        {
            if (obstacles[i].activeSelf ==  false)
            {
                return false;
            }
        }
        return true;
    }

    void Excute()
    {
        StartCoroutine(ActiveObstacle());
    }

    public IEnumerator ActiveObstacle()
    {
        while(true)
        {
            random = Random.Range(0, obstacles.Count);

            // 현재 게임 오브젝트가 활성화되어 있는지 확인합니다.
            while (obstacles[random].activeSelf == true)
            {
                // 현재 리스트에 있는 모든 게임 오브젝트가 활성화되어 있는지 확인합니다.

                if(ExmineActive())
                {
                    // 모든 게임 오브젝트가 활성화되어 있다면 게임 오브젝트를 새로
                    // 생성한 다음 obstacles 리스트에 넣어줍니다.
                    GameObject clone = Instantiate(Resources.Load<GameObject>(obstacleNames[Random.Range(0, obstacleNames.Length)]), transform);

                    clone.name = clone.name.Replace("(Clone)", "");

                    clone .SetActive(false);

                    obstacles.Add (clone);
                }

                // 현재 인덱스에 있는 게임 오브젝트가 활성화되어 있으면
                // random 변수의 값을 +1 해서 다시 검색합니다
                random = (random + 1) % obstacles.Count;
            }

            obstacles[random].transform.position = transforms[Random.Range(0, transforms.Length)].position;

            obstacles[random].SetActive(true);

            yield return CoroutineCache.WaitForSoconds(5.0f);
        }
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.START, Excute);
        State.UnSubscribe(Condition.FINISH, Release);
    }
}
