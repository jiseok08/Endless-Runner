using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] GameObject timePanel;
    [SerializeField] GameObject resultPanel;
    [SerializeField] GameObject startButton;

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.START, ExecuteInterface);
        GameEvents.Subscribe(Condition.FINISH, FinishInterface);
    }

    public void ExecuteInterface()
    {
        startButton.SetActive(false);
    }

    public void FinishInterface()
    {
        timePanel.SetActive(false);
        resultPanel.SetActive(true);
    }

    private void OnDisable()
    {
        GameEvents.UnSubscribe(Condition.START, ExecuteInterface);
        GameEvents.UnSubscribe(Condition.FINISH, FinishInterface);
    }
}
