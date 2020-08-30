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
            if (_instance == null)
            {
                _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                if (_instance == null)
                {
                    Resources.LoadAll("ScriptableObjects", typeof(T));
                    _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                }
            }
            return _instance;
        }
    }
}
