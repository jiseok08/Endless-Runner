using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] float startCycle;
    [SerializeField] float minCycle;
    [SerializeField] float cycleDecrease;

    [SerializeField] int createCount;
    [SerializeField] int tripleProb;
    [SerializeField] int standardSec;

    [SerializeField] string longObstacleName;
    [SerializeField] string [ ] obstacleNames;

    [SerializeField] Transform [ ] transforms;

    float cycle;
    float nextTime;

    int stepCount;
    int random = 0;

    List<GameObject> obstacles = new List<GameObject>();
    List<GameObject> longObstacles = new List<GameObject>();

    void Awake()
    {
        ConfigLoader.Load();
        var c = ConfigLoader.Config.ObstacleManager;

        startCycle = c.startCycle;
        minCycle = c.minCycle;
        cycleDecrease = c.cycleDecrease;

        createCount = c.createCount;
        obstacles.Capacity = c.obstacles_Capacity;
        longObstacles.Capacity = c.longObstacles_Capacity;
        tripleProb = c.tripleProb;
        standardSec = c.standardSec;
        
        longObstacleName = c.longObstacleName;
        obstacleNames = c.obstacleNames;

        cycle = startCycle;
        nextTime = Time.time + standardSec;
        stepCount = 0;
    }
    

    private void OnEnable()
    {
        State.Subscribe(Condition.START, Excute);
        State.Subscribe(Condition.FINISH, Release);
    }

    void Release()
    {
        StopAllCoroutines();
    }

    private void Start()
    {
        for (int i = 0; i < createCount; i++)
        {
            GameObject clone = Instantiate(Resources.Load<GameObject>(obstacleNames[Random.Range(0, obstacleNames.Length)]), transform);

            clone.name = clone.name.Replace("(Clone)", "");

            clone.SetActive(false);

            obstacles.Add(clone);
        }
    }

    bool ExamineActive(List<GameObject> list)
    {
        for (int i = 0; i < list.Count;i++)
        {
            if (list[i].activeSelf ==  false)
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

    void Create(int obstacleIndex, int positionIndex)
    {
        // 현재 게임 오브젝트가 활성화되어 있는지 확인합니다.
        while (obstacles[obstacleIndex].activeSelf == true)
        {
            // 현재 리스트에 있는 모든 게임 오브젝트가 활성화되어 있는지 확인합니다.

            if (ExamineActive(obstacles))
            {
                // 모든 게임 오브젝트가 활성화되어 있다면 게임 오브젝트를 새로
                // 생성한 다음 obstacles 리스트에 넣어줍니다.
                GameObject clone = Instantiate(Resources.Load<GameObject>(obstacleNames[Random.Range(0, obstacleNames.Length)]), transform);

                clone.name = clone.name.Replace("(Clone)", "");

                clone.SetActive(false);

                obstacles.Add(clone);

                obstacleIndex = obstacles.Count - 1;

                break;
            }

            // 현재 인덱스에 있는 게임 오브젝트가 활성화되어 있으면
            // random 변수의 값을 +1 해서 다시 검색합니다
            obstacleIndex = (obstacleIndex + 1) % obstacles.Count;
        }

        obstacles[obstacleIndex].transform.position = transforms[positionIndex].position;

        obstacles[obstacleIndex].SetActive(true);
    }

    public IEnumerator ActiveObstacle()
    {
        while(true)
        {
            random = Random.Range(0, obstacles.Count);

            int positionIndex = Random.Range(0, transforms.Length);

            if (Time.time >= nextTime)
            {
                cycle = Mathf.Max(minCycle, cycle - cycleDecrease); // 장애물 생성 주기 감소 (감소한 값이 최소값 이하일때는 최소값으로 변환)

                Debug.Log(cycle); 

                nextTime += standardSec;

                stepCount++;
            }

            Create(random, positionIndex);

            if (stepCount >= 1) // 기준 시간 이후부터 2개씩 생성
            {
                Create((random + 1) % obstacles.Count, (positionIndex + 1) % transforms.Length);

                if (stepCount >= 2) // 3개씩 나오는 패턴 등장 
                {
                    if (Random.Range(0, tripleProb) == 0)
                    {
                        if (ExamineActive(longObstacles)) // 비활성화된 객체 탐색
                        {
                            GameObject clone = Instantiate(Resources.Load<GameObject>(longObstacleName), transform); // 장애물 생성

                            clone.name = clone.name.Replace("(Clone)", "");

                            longObstacles.Add(clone); // 리스트에 저장

                            clone.transform.position = transforms[(positionIndex + 2) % transforms.Length].position;

                            clone.SetActive(true);
                        }
                        else // 비활성화된 장애물이 있다면
                        {
                            for (int i = 0; i < longObstacles.Count; i++)
                            {
                                if (longObstacles[i].activeSelf == false) // longObstacles를 탐색하여 비활성화된 장애물 활성화
                                {
                                    longObstacles[i].transform.position = transforms[(positionIndex + 2) % transforms.Length].position;

                                    longObstacles[i].SetActive(true);

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            yield return CoroutineCache.WaitForSeconds(cycle);
        }
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.START, Excute);
        State.UnSubscribe(Condition.FINISH, Release);
    }
}
