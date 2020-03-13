using UnityEngine;
using System;

public static class Vector2Ex {
    public static float Distance(this Vector2 from, Vector2 to) {
        return Vector2.Distance(from, to);
    }

    public static Vector2 Diff(this Vector2 from, Vector2 to) {
        return to - from;
    }

    public static Vector2 Reverse(this Vector2 vector) {
        return -1f * vector;
    }

    public static Vector2 Overwrite(this Vector2 vector, float? x = null, float? y = null) {
        return new Vector2(
            x ?? vector.x,
            y ?? vector.y
        );
    }

    public static Vector2 Add(this Vector2 vector, float? x = null, float? y = null) {
        return new Vector2(
            x + vector.x ?? vector.x,
            y + vector.y ?? vector.y
        );
    }

    public static Vector2 Map(this Vector2 vector, Func<float, float> selector) {
        return new Vector2(
            selector(vector.x),
            selector(vector.y)
        );
    }

    public static Vector2 Direction(this Vector2 from, Vector2 to) {
        return (to - from).normalized;
    }

    public static float ToDirectionDegree(this Vector2 direction) {
        return (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;
    }

    public static Direction2D4 ToDirection4(this Vector2 direction) {
        var dirNum = direction.ToDirectionNum(4);
        if (dirNum == 0)
            return Direction2D4.Zero;
        else if (dirNum == 1)
            return Direction2D4.Top;
        else if (dirNum == 2)
            return Direction2D4.Left;
        else if (dirNum == 3)
            return Direction2D4.Bottom;
        else if (dirNum == 4)
            return Direction2D4.Right;
        else
            Debug.LogError($"illigal direction: {dirNum}");

        return Direction2D4.Zero;
    }

    public static Direction2D8 ToDirection8(this Vector2 direction) {
        var dirNum = direction.ToDirectionNum(4);
        if (dirNum == 0)
            return Direction2D8.Zero;
        else if (dirNum == 1)
            return Direction2D8.Top;
        else if (dirNum == 2)
            return Direction2D8.TopLeft;
        else if (dirNum == 3)
            return Direction2D8.Left;
        else if (dirNum == 4)
            return Direction2D8.BottomLeft;
        else if (dirNum == 5)
            return Direction2D8.Bottom;
        else if (dirNum == 6)
            return Direction2D8.BottomRight;
        else if (dirNum == 7)
            return Direction2D8.Right;
        else if (dirNum == 8)
            return Direction2D8.TopRight;
        else
            Debug.LogError($"illigal direction: {dirNum}");

        return Direction2D8.Zero;
    }

    private static int ToDirectionNum(this Vector2 direction, int dirCount) {
        if (direction == Vector2.zero) {
            return 0;
        }
        var angle = Vector2.SignedAngle(Vector2.down, direction) + 180f;
        var partAngle = 360f / dirCount;

        // ずらした位置から評価
        var dirAngle = angle + (partAngle / 2);
        if (dirAngle > 360) {
            dirAngle -= 360;
        }
        return Mathf.Min(Mathf.FloorToInt(dirAngle / partAngle) + 1, dirCount);
    }
}
