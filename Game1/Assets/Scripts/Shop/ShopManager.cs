using System.Collections.Generic;
using UnityEngine; // °íÄ¡±â

public class ShopManager : MonoBehaviour, ITargetHaver
{
    [SerializeField] List<ItemData> itemDatas = new List<ItemData>();

    [SerializeField] GameObject itemProfilePrefab;

    [SerializeField] ShopExplainZoneUI explainZone;

    [SerializeField] Transform spwanPoint;

    [SerializeField] InventoryManager inventoryManager;

    private void Awake()
    {
        foreach (ItemData item in itemDatas)
        {
            ItemProfileUI profile = Instantiate(itemProfilePrefab, spwanPoint).GetComponent<ItemProfileUI>();

            profile.SetTarget(item, this);

            // itemDatas.Remove(item);
        }
    }

    public void ChangeTarget(ItemData newTarget)
    {
        explainZone.ChangeTarget(newTarget);
    }

    public bool Buy(ItemData item)
    {
        if (CoinManager.Instance.CompareCoin(item.Price))
        {
            inventoryManager.AddItem(item);

            itemDatas.Remove(item);

            return true;
        }

        return false;
    }

    public bool IsEmpty()
    {
        if (itemDatas.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public ItemData FilstData()
    {
        return itemDatas[0];
    }

    public void TargetReturn(ItemData target)
    {
        
    }
}
