using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Async;
using DG.Tweening;
using TMPro;

public class SandglassUI : MonoBehaviour {
    [SerializeField] private Image body = default;
    [SerializeField] private RectTransform texts = default;
    [SerializeField] private RectTransform bottomSandMask = default;
    [SerializeField] private Image bottomSand = default;
    [SerializeField] private RectTransform upSandMask = default;
    [SerializeField] private Image upSand = default;
    [SerializeField] private Image sandParticle = default;
    [SerializeField] private Color reverseColor = default;
    [SerializeField] private Color damageColor = default;
    [SerializeField] private TextMeshProUGUI remainingTimeLabel = default;
    [SerializeField] private TextMeshProUGUI reverseTimeLabel = default;
    [SerializeField] private TextMeshProUGUI addTimeLabel = default;
    [SerializeField] private Animator addTimeLabelAnim = default;
    [SerializeField] private TextMeshProUGUI damageTimeLabel = default;
    [SerializeField] private Animator damageTimeLabelAnim = default;

    private readonly Color defaultColor = Color.white;
    private SandUI upSandUI = default;
    private SandUI bottomSandUI = default;
    private Tweener damageSandglassTween = default;
    private bool isReversing = false;

    private void Start() {
        upSandUI = new SandUI(upSand, upSandMask);
        bottomSandUI = new SandUI(bottomSand, bottomSandMask);

        addTimeLabel.text = "+" + Mathf.CeilToInt(GameConfig.ReverseTimePerPoint).ToString();
        damageTimeLabel.text = "-" + Mathf.CeilToInt(GameConfig.DamageTimePerPoint).ToString();
    }

    public void OnChangeRate(float remainingTime, float reverseTime) {
        var passTimeRate = remainingTime / GameConfig.TimeLimit;
        var remainingTimeRate = 1f - passTimeRate;

        remainingTimeLabel.text = Mathf.CeilToInt(remainingTime).ToString();
        reverseTimeLabel.text = Mathf.CeilToInt(reverseTime).ToString();

        if (!isReversing) {
            upSandUI.ChangeRate(remainingTimeRate);
            bottomSandUI.ChangeRate(passTimeRate);
        } else {
            upSandUI.ChangeRate(passTimeRate);
            bottomSandUI.ChangeRate(remainingTimeRate);
        }
    }

    public void Stop() {
        sandParticle.gameObject.SetActive(false);
    }

    public void Show() {
        body.gameObject.SetActive(true);
        texts.gameObject.SetActive(true);
    }

    public void Hide() {
        body.gameObject.SetActive(true);
        texts.gameObject.SetActive(true);
    }

    public void Reverse() {
        isReversing = true;
        reverseTimeLabel.gameObject.SetActive(true);
        sandParticle.gameObject.SetActive(false);
        body.rectTransform.DOKill();
        body.rectTransform.Rotate(new Vector3(0, 0, -170f));
        body.rectTransform
            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f)
            .OnComplete(() => {
                sandParticle.gameObject.SetActive(true);
            });
        ChangeColor(reverseColor);
        addTimeLabelAnim.Play("Emit", 0, 0f);
    }

    public void AddTime() {
        addTimeLabelAnim.Play("Emit", 0, 0f);
    }

    public void EndReverse() {
        isReversing = false;
        reverseTimeLabel.gameObject.SetActive(false);
        sandParticle.gameObject.SetActive(false);
        body.rectTransform.DOKill();
        body.rectTransform.Rotate(new Vector3(0, 0, 170f));
        body.rectTransform
            .DOLocalRotate(new Vector3(0, 0, 0), 0.5f)
            .OnComplete(() => {
                sandParticle.gameObject.SetActive(true);
            });
        ChangeColor(defaultColor);
    }

    public void Damage() {
        var toColor = isReversing ? reverseColor : defaultColor;
        var fromColor = damageColor;
        DOTween.Kill(damageSandglassTween);
        damageSandglassTween = DOTween.To(() => fromColor, color => ChangeColor(color), toColor, 1f);
        damageTimeLabelAnim.Play("Emit", 0, 0f);
    }

    private void ChangeColor(Color color) {
        body.color = color;
        sandParticle.color = color;
        upSand.color = color;
        bottomSand.color = color;
    }
}

class SandUI {
    public Image image;
    public RectTransform mask;
    public float imageY;
    public float maskY;
    public float height;

    public SandUI(Image _image, RectTransform _mask) {
        image = _image;
        mask = _mask;
        imageY = _image.rectTransform.localPosition.y;
        maskY = _mask.localPosition.y;
        height = _mask.rect.height;
    }

    public void ChangeRate(float rate) {
        var diffY = rate * height;
        mask.localPosition = mask.localPosition.Overwrite(y: maskY - diffY);
        image.rectTransform.localPosition = image.rectTransform.localPosition.Overwrite(y: imageY + diffY);
    }
}
