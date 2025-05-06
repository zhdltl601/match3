using System;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Custom.LowLevel.LoopSystem
{
    public class Example : MonoBehaviour
    {
        //public struct MyUpdateLoop
        //{
        //    public static Action UpdateAction;
        //    public static void Fucker()
        //    {
        //        Debug.Log("fucker update");
        //        UpdateAction?.Invoke();
        //    }
        //}
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        //private static void Initialize()
        //{
        //    PlayerLoopSystem myLoop = CustomPlayerLoop.CreateLoopSystem(typeof(MyUpdateLoop), MyUpdateLoop.Fucker);
        //    CustomPlayerLoop.RegisterCustomLoop(typeof(UnityEngine.PlayerLoop.Update), myLoop);
        //}
    }
}
