using System;
using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = true;
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType<T>();
            }
            return instance;
        }

    }

    private void Awake()
    {
        if (global)
        {
            DontDestroyOnLoad(this.gameObject);
            global = false;
        }
        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}