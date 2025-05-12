using UnityEngine;
using System.Reflection;
using System;

[DefaultExecutionOrder(-200)]
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    //private static class SingletonPresetManager 
    //{
    //    private static T preset = null;
    //    public static T GetPreset => preset;
    //}

    protected virtual MonoSingletonFlags SingletonFlag { get; }
    private static T _instance = null;

    private static bool IsShuttingDown { get; set; }
    public static T Instance
    {
        get
        {
            if (_instance is null)
            {
                if (IsShuttingDown) return null;

                //if (singletonFlag.HasFlag(MonoSingletonFlags.SingletonPreset)) _instance = GetPresetSingleton();
                else _instance = RuntimeInitialize();
            }
            return _instance;
        }
    }
    private static T GetPresetSingleton() => default;
    private static T RuntimeInitialize()
    {
        GameObject gameObject = new(name: "Runtime_Singleton_" + typeof(T).Name);
        T result = gameObject.AddComponent<T>();
        Debug.LogWarning("Runtime_Singleton_" + typeof(T).Name);
        return result;
    }
    protected virtual void Awake()
    {
        //check two singleton error
        if (_instance is not null)
        {
            Destroy(gameObject);
            throw new InvalidOperationException("[ERROR]TwoSingletons_" + typeof(T).Name);
        }

        //custom singleton attribute setting
        if (SingletonFlag.HasFlag(MonoSingletonFlags.DontDestroyOnLoad)) DontDestroyOnLoad(gameObject);
        if (SingletonFlag.HasFlag(MonoSingletonFlags.Hide)) gameObject.hideFlags = HideFlags.HideInHierarchy;

#if UNITY_EDITOR
        Debug.Log($"[Singleton_Awake] [type : {typeof(T).Name}] [name : {gameObject.name}]");
#endif
        _instance = this as T;

    }
    protected virtual void OnDestroy()
    {
        if (_instance == this) _instance = null;
    }
    protected virtual void OnApplicationQuit()
    {
        IsShuttingDown = true;
    }
}