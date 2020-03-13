using UnityEngine;
using UniRx;
using UniRx.Async;

class SpringSandglass : MonoBehaviour {
    [SerializeField] private Sandglass gameTimer = default;
    [SerializeField] private Sandglass reverseTimer = default;
    [SerializeField] private SandglassUI UI = default;

    private void Start() {
        InitTimer();
        reverseTimer.OnTimerCompleted
            .Subscribe(_ => OnEndReverse())
            .AddTo(this.gameObject);
        gameTimer.OnTimerCompleted
            .Subscribe(_ => OnEndTimer())
            .AddTo(this.gameObject);
    }

    private void Update() {
        if (gameTimer.IsCounting) {
            UI.OnChangeRate(GetRemainingTime(), GetReverseRemainingTime());
        }
    }

    private void OnEndReverse() {
        gameTimer.Normal();
        reverseTimer.SetTime(0f);
        UI.EndReverse();
        Debug.Log("end reverse");
    }

    private void OnEndTimer() {
        UI.OnChangeRate(GetRemainingTime(), GetReverseRemainingTime());
        UI.Stop();
        GameManager.Instance.EndGame().Forget();
    }

    private void InitTimer() {
        reverseTimer.SetTime(0f);
        gameTimer.SetTime(GameConfig.TimeLimit);
    }

    public void StartTimer() {
        InitTimer();
        gameTimer.StartTimer();
    }

    public float GetRemainingTime() {
        return gameTimer.RemainingTime;
    }

    public float GetReverseRemainingTime() {
        return reverseTimer.RemainingTime;
    }

    public void Reverse() {
        reverseTimer.AddTime(GameConfig.ReverseTimePerPoint);
        if (!reverseTimer.IsCounting) {
            reverseTimer.StartTimer();
            gameTimer.Reverse();
            Debug.Log("start reverse");
            UI.Reverse();
        } else {
            UI.AddTime();
        }
    }

    async public UniTask Damage() {
        if (!reverseTimer.IsCounting) {
            gameTimer.MinusRemainingTime(GameConfig.DamageTimePerPoint);
        } else {
            reverseTimer.MinusRemainingTime(GameConfig.DamageTimePerPoint);
        }
        // タイマーの更新を待つため1フレームだけ待つ
        await UniTask.DelayFrame(1);
        UI.Damage();
    }
}
