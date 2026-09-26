using UnityEngine;
using UnityEngine.UI;

public class InventoryExplainZoneUI : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;

    [SerializeField] ItemData target;

    [SerializeField] Text itemProfileName;
    [SerializeField] Text description;
    [SerializeField] Image icon;
    [SerializeField] EquipButtonUI equipButton;

    private void Awake()
    {
        equipButton = GetComponentInChildren<EquipButtonUI>();

        if (inventoryManager != null)
        {
            DataSet();
        }
    }

    private void DataSet()
    {
        if (target == null)
        {
            itemProfileName.text = null;
            description.text = null;
            icon.sprite = null;

            return;
        }

        itemProfileName.text = target.ItemName;
        description.text = target.Description;
        icon.sprite = target.Icon;
    }

    public void ChangeTarget(ItemData newTarget)
    {
        target = newTarget;

        DataSet();

        equipButton.ButtonUpdate();
    }

    public void Equip()
    {
        inventoryManager.Equip(target);
    }

    public void UnEnquip()
    {
        inventoryManager.UnEquip(target);
    }

    public bool IsEquipped()
    {
        return inventoryManager.IsEquip(target);
    }
}
