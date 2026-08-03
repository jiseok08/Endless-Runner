using UnityEngine;
using UnityEngine.Events;

public class Road : MonoBehaviour, ICollidable
{
    [SerializeField] UnityEvent callback = new UnityEvent();

    public void AddCallback(UnityAction action)
    {
        callback.AddListener(action);
    }

    public void RemoveCallback(UnityAction action)
    {
        callback.RemoveListener(action);
    }

    public void OnInteract()
    {
        if(callback != null)
        {
            callback.Invoke();
        }
    }
}
