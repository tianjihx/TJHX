using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError(typeof(T) + "单例未被实例化！");
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        _instance = GetComponent<T>();
    }
}
