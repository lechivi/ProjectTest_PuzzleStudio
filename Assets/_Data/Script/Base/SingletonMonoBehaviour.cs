using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T instance;
    private bool isInitedSingleton = false;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Object.FindObjectOfType<T>();

                if (instance == null)
                {
                    Debug.LogError($"No {typeof(T).Name} Singleton Instance.");
                }
            }

            return instance;
        }
    }

    public static bool HasInstance
    {
        get
        {
            return (instance != null);
        }
    }

    public bool IsInitedSingleton { get => isInitedSingleton; }

    protected virtual void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = (T)this;
            //DontDestroyOnLoad(this);
            return true;
        }
        else if (instance == this)
        {
            //DontDestroyOnLoad(this);
            return true;
        }

        Object.Destroy(this);
        return false;
    }

    public void Init(bool force = false)
    {
        if (this.isInitedSingleton && !force)
            return;
        OnInited();
        this.isInitedSingleton = false;
    }

    public virtual void OnInited()
    {

    }
}


