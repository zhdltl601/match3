using System.Collections.Generic;
using UnityEngine;

public static class IMGUIDebug
{
    public enum DebugKey
    {
        none,
        temp1,
        temp2,
        temp3
    }
    static IMGUIDebug()
    {
        IMGUIMono.BeforeUpdate -= ClearList;
        IMGUIMono.BeforeUpdate += ClearList;
        static void ClearList()
        {
            worldInfoList.Clear();
        }
    }
    //todo : needclear
    private static readonly List<DebugWorldInfo> worldInfoList = new List<DebugWorldInfo>();
    public static void DebugText<T>(T target, int index, DebugKey key = DebugKey.none)
    {
        IMGUIMono imguiMono = IMGUIMono.Instance;
    }
    public static void DebugTextWorld<T>(T target, Vector3 worldPosition, DebugKey key = DebugKey.none)
    {
        IMGUIMono imguiMono = IMGUIMono.Instance;
        DebugWorldInfo debugWorldInfo = new DebugWorldInfo(worldPosition, target.ToString());
        worldInfoList.Add(debugWorldInfo);
        imguiMono.WorldInfoEnumerable = worldInfoList;
    }
}
