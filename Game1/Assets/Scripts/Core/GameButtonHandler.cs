using UnityEngine;

public class GameButtonHandler : Singleton<GameButtonHandler>
{
    public void StartGame()
    {
        GameEvents.Publish(Condition.START);
        AudioManager.Instance.ScenerySound("Execute");
        AudioManager.Instance.PlayEffect("Enter Button");
    }

    public void RestartGame()
    {
        GameEvents.Publish(Condition.RESET);
        AudioManager.Instance.PlayEffect("Enter Button");
    }
}
