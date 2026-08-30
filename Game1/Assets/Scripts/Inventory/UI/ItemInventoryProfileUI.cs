using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryProfileUI : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;

    [SerializeField] ItemData target;

    [SerializeField] Image icon;

    public void ChangeTarget()
    {
        inventoryManager.ChangeTarget(target);
    }
}
