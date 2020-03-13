using UnityEngine;
using System;

public static class Vector3Ex {
    public static Vector3 Direction(this Vector3 from, Vector3 to) {
        return (to - from).normalized;
    }

    public static float Distance(this Vector3 from, Vector3 to) {
        return Vector3.Distance(from, to);
    }

    public static Vector3 Diff(this Vector3 from, Vector3 to) {
        return to - from;
    }

    public static Vector3 Reverse(this Vector3 vector) {
        return -1f * vector;
    }

    public static Vector3 Overwrite(this Vector3 vector, float? x = null, float? y = null, float? z = null) {
        return new Vector3(
            x ?? vector.x,
            y ?? vector.y,
            z ?? vector.z
        );
    }

    public static Vector3 Add(this Vector3 vector, float? x = null, float? y = null, float? z = null) {
        return new Vector3(
            x + vector.x ?? vector.x,
            y + vector.y ?? vector.y,
            z + vector.z ?? vector.z
        );
    }

    public static Vector3 Map(this Vector3 vector, Func<float, float> selector) {
        return new Vector3(
            selector(vector.x),
            selector(vector.y),
            selector(vector.z)
        );
    }
}
