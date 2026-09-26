using UnityEngine;
using UnityEngine.UI;

public class EquippedItemProfile : MonoBehaviour
{
    [SerializeField] ItemData target;
    [SerializeField] Image image;

    [SerializeField] InventoryExplainZoneUI inventoryExplainZoneUI;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void ReturnTarget()
    {
        inventoryExplainZoneUI.ChangeTarget(target);
    }

    public void TargetUpdate()
    {
        target = null;

        image.sprite = null;
    }

    public void TargetUpdate(ItemData newTarget)
    {
        target = newTarget;

        if (newTarget == null)
        {
            image.sprite = null;

            return;
        }

        image.sprite = target.Icon;
    }
}
