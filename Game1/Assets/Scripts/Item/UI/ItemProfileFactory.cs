using UnityEngine; // 여기부터

public class ItemProfileFactory : MonoBehaviour
{
    public void Create(GameObject ItemProfile, Transform spwanPoint)
    {
        ItemProfileUI profile = Instantiate(ItemProfile, spwanPoint).GetComponent<ItemProfileUI>();
    }
}
