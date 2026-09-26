using UnityEngine;

public class ItemPanelController : MonoBehaviour
{
    [SerializeField] GameObject ItemPanel;

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.RESET, ActiveTrue);
        GameEvents.Subscribe(Condition.START, ActiveFalse);
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
        GameEvents.UnSubscribe(Condition.RESET, ActiveTrue);
        GameEvents.UnSubscribe(Condition.START, ActiveFalse);
    }
}
