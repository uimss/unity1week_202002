using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx.Async;

public class ScreenMaskUI : MonoBehaviour {
    [SerializeField] private Image panel = default;

    async public UniTask FadeIn(float duration) {
        panel.gameObject.SetActive(true);
        panel.color = panel.color.Overwrite(a: 0f);
        await panel.DOFade(1f, 1f).ToAwaiter();
    }

    async public UniTask FadeOut(float duration) {
        panel.gameObject.SetActive(true);
        panel.color = panel.color.Overwrite(a: 1f);
        await panel.DOFade(0f, duration).ToAwaiter();
        panel.gameObject.SetActive(false);
    }
}
