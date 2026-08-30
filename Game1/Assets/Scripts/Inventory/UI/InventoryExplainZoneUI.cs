using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryExplainZoneUI : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;

    [SerializeField] ItemData target;

    [SerializeField] Text itemProfileName;
    [SerializeField] Text description;
    [SerializeField] Image icon;

    private void Awake()
    {
        if (inventoryManager != null)
        {
            DataSet();
        }
    }

    private void OnEnable()
    {
        inventoryManager.CreateCheak();
    }

    private void DataSet()
    {
        if (target != null)
        {
            itemProfileName.text = target.ItemName;

            description.text = target.Description;

            icon.sprite = target.Icon;
        }
    }

    public void ChangeTarget(ItemData newTarget)
    {
        target = newTarget;

        DataSet();
    }

    public void Equip()
    {
        inventoryManager.Equip(target);
    }
}
