
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    private void Awake()
    {
        //Enforce Simple Singleton Design Pattern
        if (Instance == null)
            Instance = this as T;
        else if (Instance != this)
            Destroy(gameObject);
        Init();
    }

    protected virtual void Init()
    {

    }
}