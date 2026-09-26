using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.START, DisableMode);
        GameEvents.Subscribe(Condition.FINISH, EnableMode);
    }

    void Start()
    {
        EnableMode();
    }

    public void DisableMode()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnableMode()
    {
        Cursor.visible = true;
        Cursor.lockState= CursorLockMode.None;
    }

    private void OnDisable()
    {
        GameEvents.UnSubscribe(Condition.START, DisableMode);
        GameEvents.UnSubscribe(Condition.FINISH, EnableMode);
    }
}
