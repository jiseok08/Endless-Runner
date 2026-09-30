using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour, ITargetHaver
{
    private Dictionary<ItemData, ItemProfileUI> profiles = new Dictionary<ItemData, ItemProfileUI>();

    [SerializeField] GameObject itemProfilePrefab;

    [SerializeField] ShopExplainZoneUI explainZone;
    [SerializeField] Transform spwanPoint;
    [SerializeField] InventoryManager inventoryManager;

    private ItemProfileFactory itemProfileFactory;

    private void Start()
    {   
        CreateShopList();
    }

    private void CreateShopList()
    {
        itemProfileFactory = new 
            ItemProfileFactory(itemProfilePrefab, spwanPoint, this);

        foreach (ItemData item in inventoryManager.ItemDatas)
        {
            if (item == null || 
                profiles.ContainsKey(item) || 
                inventoryManager.HasItem(item))
            {
                continue;
            }

            ItemProfileUI profile = itemProfileFactory.Create(item);

            profiles.Add(item, profile);
        }
    }

    public bool Buy(ItemData item)
    {
        if (item == null || inventoryManager.HasItem(item))
        {
            Debug.LogError("구매할 수 없는 아이템입니다.");
            return false;
        }

        if (!profiles.TryGetValue(item, out ItemProfileUI profile))
        {
            Debug.LogError("아이템 프로필을 찾을 수 없습니다.");
            return false;
        }

        if (!CoinManager.Instance.TrySpendCoin(item.Price))
        {
            Debug.LogError("코인이 부족합니다.");
            return false;
        }

        inventoryManager.AddItem(item);

        Destroy(profile.gameObject);

        profiles.Remove(item);

        return true;
    }

    public void ChangeTarget(ItemData newTarget)
    {
        explainZone.ChangeTarget(newTarget);
    }
}