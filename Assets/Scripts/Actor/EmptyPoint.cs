using UnityEngine;
using DG.Tweening;

class EmptyPoint : StaticGroundObj {
    [SerializeField] Transform shadow = default;
    [SerializeField] Transform shadowMask = default;
    [SerializeField] SpriteRenderer mRenderer = default;

    public void Remove() {
        shadow.gameObject.SetActive(false);
        shadowMask.gameObject.SetActive(false);
        mRenderer
            .DOFade(0, 1f)
            .OnComplete(() => {
                foreach (Transform child in transform) {
                    Destroy(child.gameObject);
                }
                Destroy(this.gameObject);
            });
    }
}
