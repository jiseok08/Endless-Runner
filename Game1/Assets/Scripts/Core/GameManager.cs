using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public void StartGame()
    {
        GameEvents.Publish(Condition.START);
        AudioManager.Instance.ScenerySound("Execute");
        AudioManager.Instance.Listener("Enter Button");
    }

    public void RestartGame()
    {
        GameEvents.Publish(Condition.RESET);
        AudioManager.Instance.Listener("Enter Button");
    }
}
