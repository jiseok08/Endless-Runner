using UnityEngine;
using UnityEngine.UI;

public interface ITargetHaver
{
    public void ChangeTarget(ItemData newItem);
}

public class ItemProfileUI : MonoBehaviour
{ 
    [SerializeField] Text itemProfileName;

    [SerializeField] Text price;
    [SerializeField] Image icon;

    private ITargetHaver parent;

    private ItemData target;

    public void SetTarget(ItemData data, ITargetHaver parence)
    {
        target = data;

        parent = parence;

        itemProfileName.text = target.ItemName;

        if (price != null)
        {
            price.text = target.Price.ToString();
        }

        icon.sprite = target.Icon;
    }

    public void ChangeExplain()
    {
        parent.ChangeTarget(target);
    }
}
