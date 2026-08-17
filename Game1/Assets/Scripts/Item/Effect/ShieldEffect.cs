using UnityEngine;

[CreateAssetMenu(fileName = "ShieldEffect", menuName = "Item/Effect/Shield")]
public class ShieldEffect : ItemEffect
{
    [SerializeField] private int shieldAmount;

    public override void Apply()
    {
        IShieldReceiver receiver = ItemManager.Instance.Registry.Get<IShieldReceiver>();

        receiver.AddShield(shieldAmount);
    }
}