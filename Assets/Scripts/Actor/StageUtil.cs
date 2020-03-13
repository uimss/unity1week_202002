using UnityEngine;

class StageUtil {
    private const float UprightRotateRadian = 0.5f;
    private const float GroundDistance = 600f;

    public static Vector3 UprightZPosition(Vector3 position) {
        var zPos = position.y * Mathf.Tan(UprightRotateRadian);
        return position.Overwrite(z: zPos);
    }

    public static Vector3 GroundZPosition(Vector3 position) {
        var zPos = Camera.main.transform.position.z + GroundDistance;
        return position.Overwrite(z: zPos);
    }
}
