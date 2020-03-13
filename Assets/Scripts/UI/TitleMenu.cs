using UnityEngine;
using UniRx.Async;
using System;
using DG.Tweening;

class TitleMenu : MonoBehaviour {
    [SerializeField] private RectTransform menu = default;
    [SerializeField] private CanvasGroup attention = default;
    [SerializeField] private CanvasGroup attentionMessage = default;

    // TODO あとで見直したい〜〜〜
    private bool canAction = false;
    private bool canActionAttention = false;

    private void Update() {
        if (canAction && Input.GetButtonDown("Jump")) {
            CloseToStartGame();
        } else if (canActionAttention && Input.anyKeyDown) {
            CloseAttentionToOpenTitleMenu();
        }
    }

    async public UniTask Open() {
        attention.gameObject.SetActive(true);
        await DOTween
            .To(
                () => 0f,
                value => attentionMessage.alpha = value,
                1f,
                1f
            )
            .ToAwaiter();

        // 待ってる最中にAttention表示が入力により終了してたら何もしない
        canActionAttention = true;
        await UniTask.Delay(TimeSpan.FromSeconds(20f));
        if (!canActionAttention) {
            return;
        }
        canActionAttention = false;

        await OpenTitleMenu();
    }

    async private UniTask OpenTitleMenu() {
        await DOTween
            .To(
                () => 1f,
                value => attention.alpha = value,
                0f,
                1f
            )
            .ToAwaiter();
        attention.gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        menu.gameObject.SetActive(true);
        canAction = true;
    }

    private void CloseToStartGame() {
        canAction = false;
        menu.gameObject.SetActive(false);
        GameManager.Instance.StartGame().Forget();
    }

    private void CloseAttentionToOpenTitleMenu() {
        canActionAttention = false;
        OpenTitleMenu().Forget();
    }
}
