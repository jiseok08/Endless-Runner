using System.Collections.Generic;
using UnityEngine; 

public class ShopManager : MonoBehaviour, ITargetHaver
{
    [SerializeField] List<ItemData> itemDatas = new List<ItemData>();
    [SerializeField] Dictionary<ItemData, ItemProfileUI> profiles = new Dictionary<ItemData, ItemProfileUI>();

    [SerializeField] GameObject itemProfilePrefab;

    [SerializeField] ShopExplainZoneUI explainZone;
    [SerializeField] Transform spwanPoint;
    [SerializeField] InventoryManager inventoryManager;

    private ItemProfileFactory itemProfileFactory;

    private void Awake()
    {
        itemProfileFactory = new ItemProfileFactory(itemProfilePrefab, spwanPoint, this);

        foreach (ItemData item in itemDatas)
        {
            if (item == null || profiles.ContainsKey(item))
            {
                continue;
            }

            ItemProfileUI profile = itemProfileFactory.Create(item);

            profiles.Add(item, profile);
        }
    }

    public void ChangeTarget(ItemData newTarget)
    {
        explainZone.ChangeTarget(newTarget);
    }

    public bool Buy(ItemData item)
    {
        if (item == null || inventoryManager.HasItem(item))
        {
            return false;
        }

        if (!CoinManager.Instance.TrySpendCoin(item.Price))
        {
            return false;
        }

        inventoryManager.AddItem(item);

        Destroy(profiles[item].gameObject);

        profiles.Remove(item);

        itemDatas.Remove(item);

        return true;
    }
}
