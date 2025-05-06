using System;
using UnityEngine;

public class GridManager : MonoSingleton<GridManager>
{
    public Vector3 Pivot => transform.position;
    [SerializeField] private float xA;
    public static Vector3 GetPosition(Vector3 currentPosition, int snap = 1)
    {
        Vector3 result = new Vector3(
            MathF.Round(currentPosition.x / snap) * snap,
            MathF.Round(currentPosition.y / snap) * snap,
            MathF.Round(currentPosition.z / snap) * snap
        );

        return result;
    }

}
