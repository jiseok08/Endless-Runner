using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public void StartGame()
    {
        State.Publish(Condition.START);
        AudioManager.Instance.ScenerySound("Execute");
        AudioManager.Instance.Listener("Enter Button");
    }

    public void RestartGame()
    {
        State.Publish(Condition.RESET);
        AudioManager.Instance.Listener("Enter Button");
    }
}
