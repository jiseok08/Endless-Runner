using UnityEngine;

public class ItemProfileFactory
{
    private readonly GameObject itemProfile;

    private readonly Transform spwanPoint;

    private readonly ITargetHaver targetHaver;

    public ItemProfileFactory(GameObject ItemProfile, Transform spwanPoint, ITargetHaver targetHaver)
    {
        this.itemProfile = ItemProfile;
        this.spwanPoint = spwanPoint;
        this.targetHaver = targetHaver;
    }

    public ItemProfileUI Create(ItemData target)
    {
        ItemProfileUI profile = Object.Instantiate(itemProfile, spwanPoint).GetComponent<ItemProfileUI>();

        profile.SetTarget(target, targetHaver);

        return profile;
    }
}
