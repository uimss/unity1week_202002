using UnityEngine;

public static class ColorEx {
    public static Color Overwrite(this Color quaternion, float? r = null, float? g = null, float? b = null, float? a = null) {
        return new Color(
            r ?? quaternion.r,
            g ?? quaternion.g,
            b ?? quaternion.b,
            a ?? quaternion.a
        );
    }

    public static Color Add(this Color quaternion, float? r = null, float? g = null, float? b = null, float? a = null) {
        return new Color(
            r + quaternion.r ?? quaternion.r,
            g + quaternion.g ?? quaternion.g,
            b + quaternion.b ?? quaternion.b,
            a + quaternion.a ?? quaternion.a
        );
    }
}
