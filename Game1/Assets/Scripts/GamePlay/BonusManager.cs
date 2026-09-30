using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BonusManager : MonoBehaviour
{
    [SerializeField] int comboCount = 0;

    int comboTime = 0;

    int stdScore;
    int startComboTime;
    int maxCombo;

    [SerializeField] Text bonusScoreText;
    [SerializeField] GameObject ComboTimePanel;
    [SerializeField] Text comboTimeText;

    WaitForSeconds textHoldingTime; 
    WaitForSeconds decreaseTime = CoroutineCache.WaitForSeconds(1f);

    Coroutine comboRoutine;
    Coroutine textRoutine;

    private void Start()
    {
        var c = ConfigManager.Instance.Config.bonusManager;

        stdScore = c.standardScore;
        maxCombo = c.maxCombo;
        startComboTime = c.startComboTime;
        textHoldingTime = CoroutineCache.WaitForSeconds(c.textHoldingTime);

        ComboTimePanel.SetActive(false);
        bonusScoreText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.RESET, ResetBonus);
        GameEvents.Subscribe(Condition.FINISH, StopAll);
        
        GameEvents.Subscribe(Condition.BONUS, Bonus);
    }

    private void Bonus()
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

        if (comboCount < maxCombo)
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
        bonusScoreText.text = "+ " + addScore;

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
        GameEvents.UnSubscribe(Condition.RESET, ResetBonus);
        GameEvents.UnSubscribe(Condition.FINISH, StopAll);

        GameEvents.UnSubscribe(Condition.BONUS, Bonus);
    }
}
