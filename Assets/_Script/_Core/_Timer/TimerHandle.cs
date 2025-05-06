using System;
using UnityEngine;

public class TimerHandle
{
    public float EndTime { get; private set; }
    public bool IsCompleted { get; private set; }

    private readonly UnityEngine.Object target;
    private Action onCompleteCallback;
    internal TimerHandle(UnityEngine.Object unityObject, float duration)
    {
        if (unityObject == null) throw new ArgumentNullException($"{unityObject} is null");

        target = unityObject;
        EndTime = duration + Time.time;
    }
    public void AddCallback<Target>(Action<Target> action)
        where Target : UnityEngine.Object
    {
        Debug.Assert(action != null, "action is null");

        onCompleteCallback +=
            () =>
            {
                Action<Target> callbackResult = action;
                Debug.Assert(callbackResult != null, "callback cast failed");

                Target targetResult = target as Target;
                Debug.Assert(targetResult != null, "target cast failed");

                callbackResult.Invoke(targetResult);
            };
    }
    internal void Update()
    {
        if (IsCompleted) return;

        bool unityObjectDead = target == null;
        bool timeOut = Time.time > EndTime;

        bool shouldKill =
            unityObjectDead ||
            timeOut;

        bool onCompleteFire =
            timeOut;

        if (onCompleteFire)
        {
            if (onCompleteCallback != null) onCompleteCallback.Invoke();
        }

        if (shouldKill)
        {
            Kill();
        }
    }
    public void Kill()
    {
        Debug.Log("killed");
        onCompleteCallback = null;
        IsCompleted = true;
    }
}
