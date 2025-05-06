using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
public static class CustomPlayerLoop
{
    /// <summary>
    /// automatically handles customLoop subscribe, desubscribe
    /// </summary>
    /// <param name="insertTarget">target type to insert into such as <see cref="Update"/></param>
    /// <param name="customLoopSystem">loop to insert</param>
    public static void RegisterCustomLoop(Type insertTarget, PlayerLoopSystem customLoopSystem)
    {
        PlayerLoopSystem currentPlayerLoopRoot = PlayerLoop.GetCurrentPlayerLoop();

        InsertLoop(ref currentPlayerLoopRoot, customLoopSystem, 0, insertTarget);
        PlayerLoop.SetPlayerLoop(currentPlayerLoopRoot);

#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= PlayModeDispose;
        EditorApplication.playModeStateChanged += PlayModeDispose;
        void PlayModeDispose(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                RemoveLoop(ref currentPlayerLoopRoot, customLoopSystem, insertTarget);
                PlayerLoop.SetPlayerLoop(currentPlayerLoopRoot);
            }
        }
#endif
    }
    public static void PrintPlayerLoop()
    {
        PlayerLoopSystem loop = PlayerLoop.GetCurrentPlayerLoop();
        PrintPlayerLoop(loop);
    }
    public static void PrintPlayerLoop(PlayerLoopSystem rootLoop)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Unity Player Loop");

        foreach (PlayerLoopSystem subSystem in rootLoop.subSystemList)
        {
            PrintSubsystem(subSystem, sb, 0);
        }

        Debug.Log(sb.ToString());

        return;

        static void PrintSubsystem(in PlayerLoopSystem system, StringBuilder sb, int level)
        {
            sb.Append(' ', level * 2).AppendLine(system.type.ToString());

            if (system.subSystemList == null || system.subSystemList.Length == 0) return;

            foreach (PlayerLoopSystem subSystem in system.subSystemList)
            {
                PrintSubsystem(subSystem, sb, level + 1);
            }
        }
    }
    public static PlayerLoopSystem CreateLoopSystem(Type loopType, PlayerLoopSystem.UpdateFunction updateFunction, PlayerLoopSystem[] subSystemList = null)
    {
        PlayerLoopSystem result = new PlayerLoopSystem
        {
            type = loopType,
            updateDelegate = updateFunction,
            subSystemList = subSystemList
        };
        return result;
    }
    public static void InsertLoop(ref PlayerLoopSystem rootLoop, in PlayerLoopSystem newLoopToInsert, int indexInsert, Type targetLoopType)
    {
        bool flagSuccessful = InitializeTargetLoopRecursive(ref rootLoop, newLoopToInsert, indexInsert, targetLoopType);
        if (!flagSuccessful)
        {
            Debug.LogError($"{targetLoopType} insert failed");
            return;
        }

        return;

        static bool InitializeTargetLoopRecursive(ref PlayerLoopSystem loop, in PlayerLoopSystem loopToInsert, int indexInsert, Type targetLoopType)
        {
            if (loop.type != targetLoopType)
            {
                if (loop.subSystemList == null) return false;
                int subSystemListLength = loop.subSystemList.Length;
                for (int i = 0; i < subSystemListLength; i++)
                {
                    if (InitializeTargetLoopRecursive(ref loop.subSystemList[i], loopToInsert, indexInsert, targetLoopType))
                        return true;
                }
                return false;
            }

            List<PlayerLoopSystem> subSystem = new List<PlayerLoopSystem>();
            if (loop.subSystemList != null)
            {
                subSystem.AddRange(loop.subSystemList);
            }
            subSystem.Insert(indexInsert, loopToInsert);
            loop.subSystemList = subSystem.ToArray();
            return true;
        }
    }
    public static void RemoveLoop(ref PlayerLoopSystem rootLoop, in PlayerLoopSystem loopToRemove, Type targetLoopType)
    {
        bool flagSuccessful = RemoveTargetLoopRecursive(ref rootLoop, in loopToRemove, targetLoopType);
        if (!flagSuccessful)
        {
            Debug.Log($"{targetLoopType} remove failed");
        }

        return;

        static bool RemoveTargetLoopRecursive(ref PlayerLoopSystem loop, in PlayerLoopSystem loopToRemove, Type targetLoopType)
        {
            if (loop.type != targetLoopType)
            {
                if (loop.subSystemList == null) return false;
                int subSystemListLength = loop.subSystemList.Length;
                for (int i = 0; i < subSystemListLength; i++)
                {
                    if (RemoveTargetLoopRecursive(ref loop.subSystemList[i], loopToRemove, targetLoopType))
                        return true;
                }
                return false;
            }

            List<PlayerLoopSystem> subSystem = new List<PlayerLoopSystem>();
            if (loop.subSystemList != null)
            {
                subSystem.AddRange(loop.subSystemList);
            }
            subSystem.Remove(loopToRemove);
            loop.subSystemList = subSystem.ToArray();
            return true;
        }
    }
}
