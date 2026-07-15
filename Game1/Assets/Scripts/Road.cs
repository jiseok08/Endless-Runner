using UnityEngine;
using UnityEngine.Events;

public class Road : MonoBehaviour, Collidable
{
    [SerializeField] UnityEvent callback;
    public void OnInteract()
    {
        if(callback != null)
        {
            callback.Invoke();
        }
    }
}
