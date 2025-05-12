using System;
using UnityEngine;

public static class VectorExtension
{
    public static bool IsOrigin(this Vector2 vector)
    {
        bool result = false;
        const float k_range = 0.4f;
        const float k_sqrRange = k_range * k_range;
        if (vector.sqrMagnitude < k_sqrRange)
        {
            result = true;
        }
        return result;
    }
    public static Vector2 GetClampedVector(this Vector2 vector)
    {
        if (vector.IsOrigin()) return Vector2.zero;

        Vector2 result;
        float dX = Mathf.Abs(vector.x);
        float dY = Mathf.Abs(vector.y);

        if (dX > dY)
        {
            bool isPositive = vector.x > 0;
            result = isPositive ? Vector2.right : Vector2.left;
        }
        else
        {
            bool isPositive = vector.y > 0;
            result = isPositive ? Vector2.up : Vector2.down;
        }

        return result;
    }
    public static Vector2Int GetClampedVectorInt(this Vector2 vector)
    {
        if (vector.IsOrigin()) return Vector2Int.zero;

        Vector2Int result;
        float dX = Mathf.Abs(vector.x);
        float dY = Mathf.Abs(vector.y);

        if (dX > dY)
        {
            bool isPositive = vector.x > 0;
            result = isPositive ? Vector2Int.right : Vector2Int.left;
        }
        else
        {
            bool isPositive = vector.y > 0;
            result = isPositive ? Vector2Int.up : Vector2Int.down;
        }

        return result;
    }
    //public static EDirection GetEDirection(this Vector2 vector)
    //{
    //    if (vector.IsOrigin()) return EDirection.Origin;
    //
    //    EDirection result;
    //    float dX = Mathf.Abs(vector.x);
    //    float dY = Mathf.Abs(vector.y);
    //
    //    if (dX > dY)
    //    {
    //        bool isPositive = vector.x > 0;
    //        result = isPositive ? EDirection.Right : EDirection.Left;
    //    }
    //    else
    //    {
    //        bool isPositive = vector.y > 0;
    //        result = isPositive ? EDirection.Up : EDirection.Down;
    //    }
    //
    //    return result;
    //}
    //public static Vector2 GetVector(this EDirection eDirection)
    //{
    //    Vector2 result;
    //    switch (eDirection)
    //    {
    //        case EDirection.Origin:
    //            result = Vector2.zero;
    //            break;
    //        case EDirection.Left:
    //            result = Vector2.left;
    //            break;
    //        case EDirection.Right:
    //            result = Vector2.right;
    //            break;
    //        case EDirection.Up:
    //            result = Vector2.up;
    //            break;
    //        case EDirection.Down:
    //            result = Vector2.down;
    //            break;
    //        default:
    //            throw new ArgumentOutOfRangeException($"{eDirection} / {Enum.GetValues(typeof(EDirection)).Length} is out of range");
    //    }
    //    return result;
    //}
    //public static Vector2Int GetVectorInt(this EDirection eDirection)
    //{
    //    Vector2Int result;
    //    switch (eDirection)
    //    {
    //        case EDirection.Origin:
    //            result = Vector2Int.zero;
    //            break;
    //        case EDirection.Left:
    //            result = Vector2Int.left;
    //            break;
    //        case EDirection.Right:
    //            result = Vector2Int.right;
    //            break;
    //        case EDirection.Up:
    //            result = Vector2Int.up;
    //            break;
    //        case EDirection.Down:
    //            result = Vector2Int.down;
    //            break;
    //        default:
    //            throw new ArgumentOutOfRangeException($"{eDirection} / {Enum.GetValues(typeof(EDirection)).Length} is out of range");
    //    }
    //    return result;
    //}
}
