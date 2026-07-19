using UnityEngine;

public class BonusZone : MonoBehaviour
{
    bool check = false;

    private void OnEnable()
    {
        check = false;
    }

    private void OnTriggerExit(Collider other)
    {
        Runner runner = other.GetComponent<Runner>();

        if (check == false && runner != null)
        {
            BonusManager.Instance.Bonus();
            
            check = true;
        }
    }
}
