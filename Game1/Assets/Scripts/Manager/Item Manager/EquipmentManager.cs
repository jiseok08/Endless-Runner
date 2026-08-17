using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField] ItemData[] itemDatas = new ItemData[2];

    private void OnEnable()
    {
        State.Subscribe(Condition.START, ItemEffectTrue);
    }

    private void ItemEffectTrue()
    {
        foreach (ItemData item in itemDatas)
        {
            if (item != null)
            {
                item.Effect.Apply();
            }
        }
    }

    private void OnDisable()
    {
        State.Subscribe(Condition.START, ItemEffectTrue);
    }
}
