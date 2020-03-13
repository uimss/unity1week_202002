using UnityEngine;
using UniRx.Async;
using System;

class GameUI : MonoBehaviour {
    [SerializeField] private TitleMenu titleMenu = default;
    [SerializeField] private EndMenu endMenu = default;
    [SerializeField] private SandglassUI sandglass = default;
    [SerializeField] private ScreenMaskUI screenMaskUI = default;
    [SerializeField] private HintMessage hintMessage = default;

    public void OpenTitleMenu() {
        titleMenu.Open().Forget();
    }

    async public UniTask OpenEndMenu(GameScore score) {
        await endMenu.Open(score);
    }

    public void ShowSandglass() {
        sandglass.Show();
    }

    public void HideSandglass() {
        sandglass.Hide();
    }

    async public UniTask FadeInMask() {
        await screenMaskUI.FadeIn(2f);
    }

    async public UniTask FadeOutMask() {
        await screenMaskUI.FadeOut(1f);
    }

    public void TryShowHintMessage() {
        hintMessage.TryShowHintMessage();
    }
}
