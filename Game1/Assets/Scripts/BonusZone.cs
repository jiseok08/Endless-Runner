using UnityEngine;

public class BonusZone : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Runner runner = other.GetComponent<Runner>();

        if (runner != null)
        {
            BonusManager.Instance.Bonus();
        }
    }
}
