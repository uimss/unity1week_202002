using UnityEngine;

public static class QuaternionEx {
    public static Quaternion Overwrite(this Quaternion quaternion, float? x = null, float? y = null, float? z = null, float? w = null) {
        return new Quaternion(
            x ?? quaternion.x,
            y ?? quaternion.y,
            z ?? quaternion.z,
            w ?? quaternion.w
        );
    }

    public static Quaternion Add(this Quaternion quaternion, float? x = null, float? y = null, float? z = null, float? w = null) {
        return new Quaternion(
            x + quaternion.x ?? quaternion.x,
            y + quaternion.y ?? quaternion.y,
            z + quaternion.z ?? quaternion.z,
            w + quaternion.w ?? quaternion.w
        );
    }
}
