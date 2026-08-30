using UnityEngine;
using UnityEngine.UI;

public interface ITargetHaver
{
    public void ChangeTarget(ItemData newItem);
}

public class ItemProfileUI : MonoBehaviour
{
    [SerializeField] ITargetHaver parent;

    [SerializeField] ItemData target;

    [SerializeField] Text itemProfileName;

    [SerializeField] Text price;
    [SerializeField] Image icon;

    private void Start()
    {
        itemProfileName.text = target.ItemName;

        if (price != null)
        {
            price.text = target.Price.ToString();
        }

        icon.sprite = target.Icon;
    }

    public void SetTarget(ItemData data, ITargetHaver parence)
    {
        target = data;

        parent = parence;
    }

    public void ChangeExplain()
    {
        parent.ChangeTarget(target);
    }
}
