using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public void Excute()
    {
        State.Publish(Condition.START);

        AudioManager.Instance.ScenerySound("Execute");

        AudioManager.Instance.Listener("Enter Button");
    }

    public void Resume()
    {
        State.Publish(Condition.RESUME);

        SceneManager.LoadScene("Intro");
    }
}
