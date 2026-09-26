using UnityEngine;

public interface IShieldReceiver
{
    void AddShield(int amount);
}

public class ShieldController : MonoBehaviour, IShieldReceiver
{
    private int shieldCount = 0;

    public void AddShield(int amount)
    {
        shieldCount += amount;
    }

    public bool TryShield()
    {
        if (shieldCount <= 0)
        {
            return false;
        }

        shieldCount--;
        return true;
    }

    private void Start()
    {
        ItemManager.Instance.Registry.Register<IShieldReceiver>(this);
    }


}
