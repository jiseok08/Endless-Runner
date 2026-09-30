using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void StartGame()
    {
        GameButtonHandler.Instance.StartGame();
    }

    public void ResetGame()
    {
        GameButtonHandler.Instance.RestartGame();
    }
}
