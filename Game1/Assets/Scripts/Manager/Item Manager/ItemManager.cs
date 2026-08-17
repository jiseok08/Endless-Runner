using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    private readonly ItemEffectTargetRegistry registry = new ItemEffectTargetRegistry();

    public ItemEffectTargetRegistry Registry => registry;


}
