using System.Collections.Generic;
using UnityEngine; // 점검 필요

public class InventoryManager : MonoBehaviour, ITargetHaver
{
    [SerializeField] List<ItemData> inventory = new List<ItemData>();

    [SerializeField] EquipmentManager equipmentManager;
    [SerializeField] InventoryExplainZoneUI ExplainZoneUI;

    [SerializeField] GameObject inventoryProfile;

    [SerializeField] Transform spwanPoint;

    private ItemData target;

    public void CreateCheak()
    {
        foreach (ItemData item in inventory)
        {
            if (item == null)
            {
                return;
            }

            ItemProfileUI profile = Instantiate(inventoryProfile, spwanPoint).GetComponent<ItemProfileUI>();

            profile.SetTarget(item, this);
        }
    }

    public void Equip(ItemData target)
    {
         equipmentManager.EquipItem(target);
    }

    public void AddItem(ItemData itemData)
    {
        Debug.Log("AddItem");

        inventory.Add(itemData);
    }

    public void ChangeTarget(ItemData newTarget)
    {
        target = newTarget;

        ExplainZoneUI.ChangeTarget(target);
    }

}
