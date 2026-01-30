using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;

    private static ScoreManager instance;

    WaitForSeconds waitForSeconds = new(0.1f);

    int score = 0;
    int highScore = 0;

    public static ScoreManager Instance { get { return instance; } }

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

        highScore = PlayerPrefs.GetInt("HighScore", 0); // HighScore 값을 가져오고 없다면 0을 반환
        highScoreText.text = "High Score : " + highScore;
    }

    private void OnEnable()
    {
        State.Subscribe(Condition.START, Execute);
        State.Subscribe(Condition.FINISH, Release);
    }

    void Execute()
    {
        StartCoroutine(Score());
    }

    void Release()
    {
        StopAllCoroutines();

        if (score > highScore)
        {
            highScore = score;

            highScoreText.text = string.Format("High Score : " + highScore);

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();   
        }
    }

    public IEnumerator Score()
    {
        while (true)
        {
            score += 1;

            scoreText.text = string.Format("Score : " + score);

            yield return waitForSeconds; // 0.1초마다 1점씩 오름
        }
    }

    public void AddScore(int bonus)
    {
        score += bonus;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score : " + score;
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.START, Execute);
        State.UnSubscribe(Condition.FINISH, Release);
    }
}
