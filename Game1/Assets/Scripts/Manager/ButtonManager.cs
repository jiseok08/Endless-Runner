using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void Excute()
    {
        GameManager.Instance.StartGame();
    }

    public void Resume()
    {
        GameManager.Instance.RestartGame();
    }
}
