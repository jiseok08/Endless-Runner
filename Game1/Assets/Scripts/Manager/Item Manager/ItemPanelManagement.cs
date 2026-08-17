using UnityEngine;

public class ItemPanelManagement : MonoBehaviour
{
    [SerializeField] GameObject ItemPanel;

    private void OnEnable()
    {
        State.Subscribe(Condition.RESET, ActiveTrue);
        State.Subscribe(Condition.START, ActiveFalse);
    }

    private void ActiveTrue()
    {
        ItemPanel.SetActive(true);
    }

    private void ActiveFalse()
    {
        ItemPanel.SetActive(false);
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.RESET, ActiveTrue);
        State.UnSubscribe(Condition.START, ActiveFalse);
    }
}
