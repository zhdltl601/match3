using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.LowLevel;
using System;
using System.Collections.Generic;

public static class TimerRunner
{
    internal static class TimerUpdate
    {
        private readonly static List<TimerHandle> timers = new List<TimerHandle>(16);
        public static void AddTimer(TimerHandle timerHandle)
        {
            timers.Add(timerHandle);
        }
        public static void UpdateFunction()
        {
            for (int i = timers.Count - 1; i >= 0; i--)
            {
                TimerHandle timer = timers[i];
                timer.Update();
                if (timer.IsCompleted)
                {
                    timers.RemoveAt(i);// todo :
                }
            }
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        PlayerLoopSystem timerLoop = CustomPlayerLoop.CreateLoopSystem(typeof(TimerUpdate), TimerUpdate.UpdateFunction);
        CustomPlayerLoop.RegisterCustomLoop(typeof(Update), timerLoop);
    }
    /// <summary>
    /// creates new timer and register with callback
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    /// <param name="callback">can be null</param>
    /// <returns>timer instance</returns>
    public static TimerHandle Register<Target>(Target target, float duration, Action<Target> callback = null)
        where Target : UnityEngine.Object
    {
        Debug.Assert(target != null, "target is null");

        TimerHandle result = new TimerHandle(target, duration);
        if (callback != null)
        {
            result.AddCallback(callback);
        }

        TimerUpdate.AddTimer(result);

        return result;
    }
    public static TimerHandle Register<Target>(Target target, float duration, Action<Target> callback, TimerHandle handle)
    {
        TimerHandle result = handle;

        //if(callback != null)

        return result;
    }
    public static TimerHandle Register(TimerHandle timerHandle, float duration)
    {
        TimerHandle result = timerHandle;
        return result;
    }
    public static TimerHandle MoonoCallback<Target>(this Target target, float duration, Action<Target> callback)
        where Target : MonoBehaviour
    {
        TimerHandle result = Register(target, duration, callback);

        return result;
    }
}