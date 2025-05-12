using System;
using UnityEngine;

public class GridManager : MonoSingleton<GridManager>//todo : for debugging, should be static class later
{
    public Vector3 Pivot => transform.position;
    public static Vector3 GetPosition(Vector3 currentPosition, int snap = 1)
    {
        Vector3 result = new Vector3(
            MathF.Round(currentPosition.x / snap) * snap,
            MathF.Round(currentPosition.y / snap) * snap,
            MathF.Round(currentPosition.z / snap) * snap
        );

        return result;
    }
    public static Vector2 GetPosition(Vector2 currentPosition, int snap = 1)
    {
        Vector2 result = new Vector2(
            MathF.Round(currentPosition.x / snap) * snap,
            MathF.Round(currentPosition.y / snap) * snap
        );

        return result;
    }
}
