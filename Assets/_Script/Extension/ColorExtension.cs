using System;
using UnityEngine;

public static class ColorExtension
{
    public static Color GetColor(this EColor colorType)
    {
        Color result = colorType switch
        {
            EColor.Red      => new Color32(250, 54, 54, 255),
            EColor.Black    => new Color32(74, 74, 74, 255),
            EColor.White    => new Color32(255, 255, 255, 255),
            EColor.None     => new Color32(255, 100, 210, 255),// todo : should i throw an exception?, def err color is pink
            _ => throw new ArgumentOutOfRangeException($"enum {colorType} is not defined")
        };
        return result;
    }
}
