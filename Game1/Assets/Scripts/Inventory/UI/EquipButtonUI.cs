using UnityEngine;
using UnityEngine.UI;

public class EquipButtonUI : MonoBehaviour
{
    [SerializeField] InventoryExplainZoneUI inventoryExplainZoneUI;

    [SerializeField] Color green;
    [SerializeField] Color red;

    Text text;
    Image image;

    public void Awake()
    {
        text = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
    }

    public void Action()
    {
        if(inventoryExplainZoneUI.IsEquipped())
        {
            inventoryExplainZoneUI.UnEnquip();
        }
        else
        {
            inventoryExplainZoneUI.Equip();
        }

        ButtonUpdate();
    }

    public void ButtonUpdate()
    {
        if(inventoryExplainZoneUI.IsEquipped())
        {
            text.text = "UnEquip";
            image.color = red;
        }
        else
        {
            text.text = "Equip";
            image.color = green;
        }
    }
}