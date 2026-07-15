using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance; 

    public static T Instance  
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindAnyObjectByType(typeof(T));

                if (instance == null)
                {
                    GameObject clone = new GameObject(typeof(T).Name);

                    instance = clone.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = (T)(object)this;

            DontDestroyOnLoad(gameObject);

            Initialize();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        } 
    }

    protected virtual void Initialize() { } 
}
