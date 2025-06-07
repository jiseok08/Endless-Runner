using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
