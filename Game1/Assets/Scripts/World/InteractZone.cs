using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ICollidable collidable = other.GetComponent<ICollidable>();

        if (collidable != null)
        {
            collidable.OnInteract();
        }
    }
}
