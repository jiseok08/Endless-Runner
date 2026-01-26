using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusZone : MonoBehaviour
{
    [SerializeField] int bonusScore;

    private void Awake()
    {
        ConfigLoader.Load();
        bonusScore = ConfigLoader.Config.BonusZone.bonusScore;
    }

    void OnTriggerExit(Collider other)
    {
        Runner runner = other.GetComponent<Runner>();

        if (runner != null)
        {
            ScoreManager.Instance.Bonus(bonusScore);
        }
    }
}
