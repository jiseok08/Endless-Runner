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
    }

    void Awake()
    {
        obstacles.Capacity = 10;

        Create();
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

            // ���� ���� ������Ʈ�� Ȱ��ȭ�Ǿ� �ִ��� Ȯ���մϴ�.
            while (obstacles[random].activeSelf == true)
            {
                // ���� ����Ʈ�� �ִ� ��� ���� ������Ʈ�� Ȱ��ȭ�Ǿ� �ִ��� Ȯ���մϴ�.

                if(ExmineActive())
                {
                    // ��� ���� ������Ʈ�� Ȱ��ȭ�Ǿ� �ִٸ� ���� ������Ʈ�� ����
                    // ������ ���� obstacles ����Ʈ�� �־��ݴϴ�.
                    GameObject clone = Instantiate(Resources.Load<GameObject>(obstacleNames[Random.Range(0, obstacleNames.Length)]), transform);

                    clone.name = clone.name.Replace("(Clone)", "");

                    clone .SetActive(false);

                    obstacles.Add (clone);
                }

                // ���� �ε����� �ִ� ���� ������Ʈ�� Ȱ��ȭ�Ǿ� ������
                // random ������ ���� +1 �ؼ� �ٽ� �˻��մϴ�
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
    }
}
