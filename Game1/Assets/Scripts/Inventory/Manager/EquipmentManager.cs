using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
    private const int SlotCount = 2;

    [SerializeField] ItemData[] equippedItems = new ItemData[SlotCount];

    [SerializeField] EquippedItemProfile[] equipProfiles = new EquippedItemProfile[SlotCount];

    private void OnEnable()
    {
        GameEvents.Subscribe(Condition.START, ApplyItemEffect);
    }

    private void ApplyItemEffect()
    {
        foreach (ItemData item in equippedItems)
        {
            if (item == null || item.Effect == null)
            {
                continue;
            }

            item.Effect.Apply();
        }
    }

    public bool EquipItem(ItemData itemData)
    {
        if (itemData == null || IsEquipped(itemData))
        {
            return false;
        }

        for (int i = 0; i < SlotCount; i++)
        {
            if (equippedItems[i] != null)
            {
                continue;
            }

            equippedItems[i] = itemData;
            equipProfiles[i].TargetUpdate(itemData);

            return true;
        }

        return false;
    }

    public bool RemoveItem(ItemData itemData)
    {
        if (itemData == null)
        {
            return false;
        }

        for (int i = 0; i < SlotCount; i++)
        {
            if (equippedItems[i] != itemData)
            {
                continue;
            }

            equippedItems[i] = null;
            equipProfiles[i].TargetUpdate(null);

            return true;
        }

        return false;
    }

    public bool IsEquipped(ItemData itemData)
    {
        if (itemData == null)
        {
            return false;
        }

        foreach (ItemData item in equippedItems)
        {
            if (item == itemData)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDisable()
    {
        GameEvents.UnSubscribe(Condition.START, ApplyItemEffect);
    }
}
