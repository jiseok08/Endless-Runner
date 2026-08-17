using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExplainZoneUI : MonoBehaviour
{
    [SerializeField] ItemData target;

    [SerializeField] Text itemProfileName;

    [SerializeField] Text description;

    [SerializeField] Text price;
    [SerializeField] Image icon;

    private void Awake()
    {
        itemProfileName.text = target.ItemName;

        description.text = target.Description;

        price.text = target.Price.ToString();

        icon.sprite = target.Icon;
    }
}
