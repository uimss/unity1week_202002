using UnityEngine;
using UniRx.Async;
using DG.Tweening;

class ShadowManager : SingletonMonoBehaviour<ShadowManager> {
    [SerializeField] private Color color = Color.black;
    [SerializeField] private Material shadowMaterial = default;
    [SerializeField] private Gradient lightGradient = default;
    [SerializeField] private ParticleSystem screenSnow = default;

    private void Start() {
        shadowMaterial.SetColor("_Color", color);
    }

    public Color GetColor() {
        return color;
    }

    public void SetColor(Color newColor) {
        color = newColor;
    }

    async public UniTask TweenSpring() {
        screenSnow.Stop();
        await DOTween
            .To(
                () => 0f,
                value => Camera.main.backgroundColor = lightGradient.Evaluate(value),
                1f,
                2f
            )
            .ToAwaiter();
    }
}
