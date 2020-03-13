using UnityEngine;
using UniRx;
using UniRx.Async;
using System.Linq;
using System;

class SpringPointGroup : MonoBehaviour {

    [SerializeField] private AudioClip se = default;
    private System.IDisposable seLoopDisposable;
    private SpringPoint[] activePoints = new SpringPoint[0];
    private SpringPoint[] enteredPoints = new SpringPoint[0];
    private const float seVolumeMin = 0.2f;
    private const float seVolumeMax = 1.0f;
    private const float seDelaySeconds = 0.35f;
    private const float seIntervalSeconds = 2.5f;

    private void Awake() {
        activePoints = GetComponentsInChildren<SpringPoint>();
    }

    private void Start() {
        transform.position = StageUtil.GroundZPosition(transform.position);
    }

    private void Update() {
        transform.position = StageUtil.GroundZPosition(transform.position);
    }

    public void OnSoundAreaEnter(SpringPoint point) {
        enteredPoints = enteredPoints.Union(Enumerable.Repeat(point, 1)).ToArray();
        TryPlaySeLoop();
    }

    public void OnSoundAreaExit(SpringPoint point) {
        enteredPoints = enteredPoints.Where(p => p != point).ToArray();
        if (enteredPoints.Length == 0) {
            TryStopSeLoop();
        }
    }

    public void OnPickPoint(SpringPoint point) {
        activePoints = activePoints.Where(p => p != point).ToArray();
        Debug.Log(activePoints.Length);
    }

    async private UniTaskVoid PlaySe() {
        foreach (var p in activePoints) {
            // 距離が近いほど音が大きくなる
            float volume;
            float pitch;
            if (GameManager.Instance.IsNearPlayer(p)) {
                volume = 1f;
                pitch = 1.2f;
            } else {
                var distance = Vector2.Distance(GameManager.Instance.GetPlayerPosition(), p.transform.position);
                var radius = p.GetSoundAreaRadius();

                volume = Mathf.Lerp(seVolumeMin, seVolumeMax, Mathf.Clamp(1.2f - distance / radius, 0.1f, 0.9f));
                pitch = 1f;
            }

            SoundManager.Instance.PlaySe(se, volume, pitch);
            await UniTask.Delay(TimeSpan.FromSeconds(seDelaySeconds));
        }
    }

    private void TryPlaySeLoop() {
        if (seLoopDisposable != null) {
            return;
        }
        PlaySe().Forget();
        seLoopDisposable = Observable
            .Interval(System.TimeSpan.FromSeconds(seIntervalSeconds))
            .Subscribe(_ => PlaySe().Forget())
            .AddTo(this.gameObject);
    }

    private void TryStopSeLoop() {
        if (seLoopDisposable == null) {
            return;
        }
        seLoopDisposable.Dispose();
        seLoopDisposable = null;
    }
}
