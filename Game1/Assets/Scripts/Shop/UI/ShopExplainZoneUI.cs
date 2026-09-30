using UnityEngine;
using UnityEngine.UI;

public class ShopExplainZoneUI : MonoBehaviour
{
    [SerializeField] ShopManager shopManager;

    [SerializeField] ItemData target;

    [SerializeField] Text itemProfileName;

    [SerializeField] Text description;

    [SerializeField] Text price;
    [SerializeField] Image icon;

    private void Awake()
    {
        DataSet();
    }

    private void DataSet()
    {
        if (target == null)
        {
            itemProfileName.text = null;

            description.text = null;

            price.text = null;

            icon.sprite = null;

            return;
        }

        itemProfileName.text = target.ItemName;

        description.text = target.Description;

        if (price != null)
        {
            price.text = target.Price.ToString();
        }

        icon.sprite = target.Icon;
    }

    public void ChangeTarget(ItemData newTarget)
    {
        target = newTarget;

        DataSet();
    }

    public void Buy()
    {
        if (!shopManager.Buy(target))
        {
            return;
        }

        target = null;

        DataSet();
    }
}
