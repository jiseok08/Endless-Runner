using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // 여기서부터 수정

public class EquipmentManager : MonoBehaviour
{
    private const int SlotCount = 2;

    [SerializeField] ItemData[] equippedItems = new ItemData[SlotCount];

    [SerializeField] Image[] equipImages = new Image[SlotCount];

    [SerializeField] int targetIndex = 0;

    private void OnEnable()
    {
        State.Subscribe(Condition.START, ApplyItemEffect);
    }

    private void ApplyItemEffect()
    {
        foreach (ItemData item in equippedItems)
        {
            if (item != null)
            {
                item.Effect.Apply();
            }
        }
    }

    public void EquipItem(ItemData itemData)
    {
        Debug.Log("작동은 함");
        for (int i = 0; i < SlotCount; i++)
        {
            if (equippedItems[i] != null)
            {
                Debug.Log("컨티뉴");

                continue;
            }

            equippedItems[i] = itemData;
            equipImages[i].sprite = itemData.Icon;
            Debug.Log("아이템 장착");

            return;
        }
    }

    public void RemoveItem(ItemData itemData)
    {
        if (equippedItems[targetIndex] != itemData)
        {
            return;
        }

        equippedItems[targetIndex] = null;
        equipImages[targetIndex].sprite = null;
    }

    private void OnDisable()
    {
        State.UnSubscribe(Condition.START, ApplyItemEffect);
    }

    public void ChangeTarget(ItemData newTarget)
    {
        throw new System.NotImplementedException();
    }
}
