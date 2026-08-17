using UnityEngine;
using UnityEngine.UI;

public class ItemProfileUI : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    [SerializeField] Text itemProfileName;

    [SerializeField] Text price;
    [SerializeField] Image icon;

    private void Awake()
    {
        itemProfileName.text = itemData.ItemName;

        price.text = itemData.Price.ToString();

        icon.sprite = itemData.Icon;
    }
}
