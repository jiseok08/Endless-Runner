using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, ITargetHaver
{
    private const string ItemSaveKey = "Inventory.Item.";

    [SerializeField] List<ItemData> inventory = new List<ItemData>();

    [SerializeField] EquipmentManager equipmentManager;
    [SerializeField] InventoryExplainZoneUI explainZoneUI;

    [SerializeField] GameObject inventoryProfilePrefab;
    [SerializeField] Transform spawnPoint;

    private Dictionary<ItemData, ItemProfileUI> profiles = new Dictionary<ItemData, ItemProfileUI>();

    private ItemProfileFactory profileFactory;

    private ItemData target;

    private void Awake()
    {
        CreateCheck();
    }

    public void CreateCheck()
    {
        if (profileFactory == null)
        {
            profileFactory = new ItemProfileFactory(inventoryProfilePrefab, spawnPoint, this);
        }

        foreach (ItemData item in inventory)
        {
            if (item == null || profiles.ContainsKey(item))
            {
                continue;
            }

            ItemProfileUI profile = profileFactory.Create(item);

            profiles.Add(item, profile);
        }
    }

    public void LoadItems(IEnumerable<ItemData> itemDatas)
    {
        foreach (ItemData item in itemDatas)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.ItemId) || inventory.Contains(item))
            {
                continue;
            }

            if (PlayerPrefs.GetInt(ItemSaveKey + item.ItemId, 0) == 1)
            {
                inventory.Add(item);
            }
        }

        CreateCheck();
    }

    public void Equip(ItemData target)
    {
        if (equipmentManager.EquipItem(target))
        {
            if (profiles.TryGetValue(target, out ItemProfileUI profile))
            {
                profile.gameObject.SetActive(false);
            }
        }
    }

    public void UnEquip(ItemData target)
    {
        if (equipmentManager.RemoveItem(target))
        {
            if (profiles.TryGetValue(target, out ItemProfileUI profile))
            {
                profile.gameObject.SetActive(true);
            }
        }
    }

    public void AddItem(ItemData itemData)
    {
        if (itemData == null || inventory.Contains(itemData))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(itemData.ItemId))
        {
            Debug.LogError("Item Id가 비어있음");

            return;
        }

        inventory.Add(itemData);

        PlayerPrefs.SetInt(ItemSaveKey + itemData.ItemId, 1);
        PlayerPrefs.Save();

        CreateCheck();
    }

    public bool HasItem(ItemData itemData)
    {
        if (itemData == null)
        {
            return false;
        }

        return inventory.Contains(itemData);
    }

    public void ChangeTarget(ItemData newTarget)
    {
        target = newTarget;

        explainZoneUI.ChangeTarget(target);
    }

    public bool IsEquip(ItemData itemData)
    {
        if (itemData == null)
        {
            return false;
        }

        return equipmentManager.IsEquipped(itemData);
    }
}