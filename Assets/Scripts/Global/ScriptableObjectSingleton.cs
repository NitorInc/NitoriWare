using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
{
    static T _instance = null;
    public static T instance
    {
        get
        {
            if (!_instance)
                _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            return _instance;
        }
    }
}
