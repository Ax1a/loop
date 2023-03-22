using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T _instance;

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        _instance = (T)(object)this;
    }
}
