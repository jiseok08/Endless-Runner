using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Item Data")] 
public class ItemData : ScriptableObject 
{
    [SerializeField] string itemName;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] int price;
    [SerializeField] Sprite icon;
    [SerializeField] ItemEffect effect;

    public string ItemName => itemName;

    public string Description => description;

    public int Price => price;

    public Sprite Icon => icon;

    public ItemEffect Effect => effect;
}
