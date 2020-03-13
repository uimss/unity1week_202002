using UnityEngine;

class StaticObj : MonoBehaviour {
    private void Start() {
        transform.position = StageUtil.UprightZPosition(transform.position);
    }
}
