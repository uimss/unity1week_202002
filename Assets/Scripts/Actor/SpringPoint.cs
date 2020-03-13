using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UniRx.Async;
using DG.Tweening;

class SpringPoint : MonoBehaviour {

    [SerializeField] private CircleCollider2D soundArea = default;
    [SerializeField] private SpriteRenderer spring = default;
    [SerializeField] private SpriteRenderer springShadow = default;
    [SerializeField] private Transform soil = default;
    [SerializeField] private bool isHidden = true;
    [SerializeField] private SpriteRenderer pickedSpring = default;
    [SerializeField] private AudioClip pickSe = default;
    [SerializeField] private AudioClip diggSe = default;
    [SerializeField] private Color drySpringColor = default;
    private SpringPointGroup pointGroup;
    private bool isPicked = false;

    private void Awake() {
        pointGroup = GetComponentInParent<SpringPointGroup>();
    }

    private void Start() {
        soundArea.OnTriggerEnter2DAsObservable()
            .Where(_ => _.GetComponent<Player>() != null)
            .Subscribe(_ => pointGroup.OnSoundAreaEnter(this))
            .AddTo(this.gameObject);
        soundArea.OnTriggerExit2DAsObservable()
            .Where(_ => _.GetComponent<Player>() != null)
            .Subscribe(_ => pointGroup.OnSoundAreaExit(this))
            .AddTo(this.gameObject);
        if (isHidden) {
            spring.gameObject.SetActive(false);
            soil.gameObject.SetActive(false);
        }
    }

    public float GetSoundAreaRadius() {
        return soundArea.radius;
    }

    public bool TryPick() {
        Debug.Log("try pick");
        if (isHidden && !soil.gameObject.activeSelf) {
            DiggSnow();
            return false;
        } else {
            Pick();
            return true;
        }
    }

    private void DiggSnow() {
        spring.gameObject.SetActive(true);
        soil.gameObject.SetActive(true);
        PlayDiggSnowParticle();
        SoundManager.Instance.PlaySe(diggSe);
    }

    private void PlayDiggSnowParticle() {
        var point = StageUtil.UprightZPosition(transform.position);
        CommonResource.Instance.diggSnowParticle.transform.position = point;
        CommonResource.Instance.diggSnowParticle.Play();
    }

    private void Pick() {
        if (isPicked) {
            Debug.LogWarning("pickが重複してよばれてるかもしれない");
            return;
        }
        isPicked = true;
        pickedSpring.gameObject.SetActive(true);
        springShadow.gameObject.SetActive(false);
        spring.transform.localScale = spring.transform.localScale.Overwrite(x: 0.7f);
        spring.color = drySpringColor;
        var originalYpos = spring.transform.position.y;

        DOTween
            .To(
                value => {
                    spring.color = spring.color.Overwrite(a: 1f - value);
                    spring.transform.position = spring.transform.position.Overwrite(y: originalYpos + (value * 10f));
                },
                0f, 1f, 2f
            )
            .SetEase(Ease.OutCirc)
            .OnComplete(() => spring.gameObject.SetActive(false));
        SoundManager.Instance.PlaySe(pickSe);
        // Disable Collider
        gameObject.layer = LayerName.IgnoreRaycast;
        soundArea.gameObject.layer = LayerName.IgnoreRaycast;
        pointGroup.OnPickPoint(this);
    }

    // スコア計算用
    public bool IsPicked() {
        return isPicked;
    }
}
