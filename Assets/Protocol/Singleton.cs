using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            string typename = typeof(T).Name;
            Debug.LogError($"More that one Instance of {typename} found.  Destroying the copy.");
            Destroy(this);
            return;
        }
        Instance = this as T;
    }
}
