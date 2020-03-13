using UnityEngine;
using DG.Tweening;

public class ShadowReceiver : MonoBehaviour {
    [SerializeField] private SpriteRenderer mRenderer = default;
    [SerializeField] private Transform mShadow = default;
    private Color originalColor;

    private void Start() {
        originalColor = mRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerName.Shadow && other.transform != mShadow) {
            mRenderer.DOKill();
            mRenderer.DOColor(originalColor * ShadowManager.Instance.GetColor(), 0.2f);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerName.Shadow && other.transform != mShadow) {
            mRenderer.DOKill();
            mRenderer.DOColor(originalColor, 0.2f);
        }
    }
}
