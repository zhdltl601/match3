using System;
using System.Collections.Generic;
using UnityEngine;

internal struct DebugWorldInfo
{
    public Vector3 worldPosition;
    public string message;

    public DebugWorldInfo(Vector3 worldPosition, string message)
    {
        this.worldPosition = worldPosition;
        this.message = message;
    }

    public Vector3 GetScreenPos()
    {
        return default;
    }
}

[DefaultExecutionOrder(-200)]
internal class IMGUIMono : MonoSingleton<IMGUIMono>
{
    protected override MonoSingletonFlags SingletonFlag => MonoSingletonFlags.DontDestroyOnLoad;
    private static bool EnableGUI { get; set; } = true;
    public IEnumerable<DebugWorldInfo> WorldInfoEnumerable { get; set; }
    public static event Action BeforeUpdate;
    [SerializeField] private float size;
    private void Update()
    {
        BeforeUpdate?.Invoke();
    }
    private void OnGUI()
    {
        if (!EnableGUI) return;

        Camera camera = Camera.main;
        int screenHeight = Screen.height;
        foreach (DebugWorldInfo item in WorldInfoEnumerable)
        {
            Vector3 screenPosition = camera.WorldToScreenPoint(item.worldPosition, Camera.MonoOrStereoscopicEye.Mono);
            screenPosition.y = screenHeight - screenPosition.y;
            Vector2 size = new Vector3(200, 200);
            Rect position = new Rect(screenPosition, size);


            //GUI.skin.label.Draw(position, )
            GUI.Label(position, item.message, GUI.skin.label);
        }
    }
}
