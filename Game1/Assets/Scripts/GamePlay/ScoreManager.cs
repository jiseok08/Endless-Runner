using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public interface IScoreMultiplierReceiver
{
    void SetScoreMultiplier(float multiplier);
}

public class ScoreManager : Singleton<ScoreManager>, IScoreMultiplierReceiver
{
    private const string SaveKey = "HighScore";

    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;

    WaitForSeconds scoreInterval;

    int score;
    int highScore;
    int baseScore;

    float multiplier = 1;
    float scoreBuffer = 0;

    private void Start()
    {
        var c = ConfigManager.Instance.Config.scoreManager;

        baseScore = c.baseScore;

        scoreInterval = CoroutineCache.WaitForSeconds(c.scoreInterval);

        highScore = PlayerPrefs.GetInt(SaveKey, 0);
        highScoreText.text = "High Score : " + highScore;

        ResetScore();

        ItemManager.Instance.Registry.Register<IScoreMultiplierReceiver>(this);
    }

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.RESET, ResetScore);  
        GameEvents.Subscribe(Condition.START, Execute);
        GameEvents.Subscribe(Condition.FINISH, Release);
    }

    void ResetScore()
    {
        score = 0;
        multiplier = 1;
        scoreBuffer = 0;
        scoreText.text = "Score : 0";
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

            highScoreText.text = "High Score : " + highScore;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();   
        }
    }

    public void SetScoreMultiplier(float multiplier)
    {
        this.multiplier = multiplier;
    }

    public IEnumerator Score()
    {
        while (true)
        {
            scoreBuffer += baseScore * multiplier;

            int addScore = Mathf.FloorToInt(scoreBuffer);

            score += addScore;

            scoreBuffer -= addScore;

            UpdateUI();

            yield return scoreInterval;
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
        {
            scoreText.text = "Score : " + score;
        }
    }

    private void OnDisable()
    {
        GameEvents.UnSubscribe(Condition.RESET, ResetScore);
        GameEvents.UnSubscribe(Condition.START, Execute);
        GameEvents.UnSubscribe(Condition.FINISH, Release);
    }
}
