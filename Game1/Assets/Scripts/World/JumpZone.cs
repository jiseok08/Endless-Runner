using UnityEngine;

public class JumpZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Runner runner = other.GetComponent<Runner>();

        if (runner != null)
        {
            runner.canJump = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Runner runner = other.GetComponent<Runner>();

        if (runner != null)
        {
            runner.canJump = false;
        }
    }
}
