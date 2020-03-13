using UnityEngine;

class GameCamera : MonoBehaviour {
    [SerializeField] private Camera mainCamera = default;
    [SerializeField] private Camera shadowCamera = default;

    private void Start() {
        mainCamera.cullingMask = LeyerMaskEx.Without(LayerName.Shadow);
        shadowCamera.gameObject.SetActive(true);
    }
}
