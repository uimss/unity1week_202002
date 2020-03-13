using UnityEngine;

class StaticGroundObj : MonoBehaviour {
    [SerializeField] private float zOffset = 0f;

    private void Start() {
        transform.position = StageUtil.GroundZPosition(transform.position).Add(z: zOffset);
    }

    private void Update() {
        transform.position = StageUtil.GroundZPosition(transform.position).Add(z: zOffset);
    }
}
