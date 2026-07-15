using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BonusManager : Singleton<BonusManager>
{
    int stdScore;

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

    private void Start()
    {
        var c = ConfigManager.Instance.Config.bonusManager;

        stdScore = c.standardScore;
        startComboTime = c.startComboTime;
        textHoldingTime = new(c.textHoldingTime);

        ComboTimePanel.SetActive(false);
        bonusScoreText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        State.Subscribe(Condition.RESET, ResetBonus);
        State.Subscribe(Condition.FINISH, StopAll);
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

    void ResetBonus()
    {
        comboRoutine = null;
        textRoutine = null;
        comboCount = 0;
        comboTime = 0;
    }

    void StopAll()
    {
        StopAllCoroutines();
        comboRoutine = null;
        textRoutine = null;
        ComboTimePanel.SetActive(false);
        bonusScoreText.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.RESET, ResetBonus);
        State.UnSubscribe(Condition.FINISH, StopAll);
    }
}
