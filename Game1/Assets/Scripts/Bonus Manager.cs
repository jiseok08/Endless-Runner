using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.CinemachineOrbitalTransposer;

public class BonusManager : MonoBehaviour
{
    [SerializeField] int stdScore;

    int comboCount = 0;

    int comboTime = 0;

    int startComboTime;

    [SerializeField] Text bonusScoreText;

    [SerializeField] GameObject ComboTimePanel;
    [SerializeField] Text comboTimeText;

    WaitForSeconds textHoldingTime; 
    WaitForSeconds decreaseTime = new WaitForSeconds(1f);

    Coroutine comboRoutine;
    Coroutine textRoutine;

    private static BonusManager instance;

    public static BonusManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ConfigLoader.Load();

        var c = ConfigLoader.Config.BonusManager;

        stdScore = c.stdScore;
        startComboTime = c.startComboTime;
        textHoldingTime = new(c.textHoldingTime);

        ComboTimePanel.SetActive(false);
        bonusScoreText.gameObject.SetActive(false);
    }

    public void Bonus()
    {
        if (comboRoutine != null) StopCoroutine(comboRoutine);
        if (textRoutine != null) StopCoroutine(textRoutine);

        comboRoutine = StartCoroutine(Combo());

        int bonusScore = stdScore * comboCount;

        textRoutine = StartCoroutine(BonusText(bonusScore));

        ScoreManager.Instance.AddScore(bonusScore);
    }

    IEnumerator Combo()
    {
        comboTime = startComboTime;

        comboTimeText.text = comboTime.ToString();

        if (comboCount < 5)
        {
            comboCount++;
        }

        if (ComboTimePanel.activeSelf == false)
        {
            ComboTimePanel.gameObject.SetActive(true);
        }

        while (comboTime > 0)
        {
            yield return decreaseTime;

            comboTime--;

            comboTimeText.text = comboTime.ToString();
        }

        comboCount = 0;

        ComboTimePanel.SetActive(false);

        comboRoutine = null;
    }

    IEnumerator BonusText(int addScore)
    {
        bonusScoreText.text = string.Format("+ " + addScore);

        if (bonusScoreText.IsActive() == false)
        {
            bonusScoreText.gameObject.SetActive(true);
        }

        yield return textHoldingTime;

        bonusScoreText.gameObject.SetActive(false);

        textRoutine = null;
    }
}
