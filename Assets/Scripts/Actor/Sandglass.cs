using UnityEngine;
using UniRx;
using System;

public class Sandglass : MonoBehaviour {

    public bool IsCounting { get; private set; } = false;
    public float RemainingTime { get; private set; } = 0f;
    public bool IsReversed { get; private set; } = false;

    private float totalTime = 10f;
    private Subject<Unit> timerStartSubject = new Subject<Unit>();
    private Subject<Unit> timerCompleteSubject = new Subject<Unit>();

    public IObservable<Unit> OnTimerStart {
        get { return timerStartSubject; }
    }

    public IObservable<Unit> OnTimerCompleted {
        get { return timerCompleteSubject; }
    }

    private void Update() {
        if (IsCounting) {
            if (!IsReversed) {
                if (RemainingTime > 0) {
                    RemainingTime = Mathf.Max(RemainingTime - Time.deltaTime, 0);
                }
                if (RemainingTime == 0) {
                    IsCounting = false;
                    RemainingTime = 0;
                    timerCompleteSubject.OnNext(Unit.Default);
                }
            } else {
                if (RemainingTime < totalTime) {
                    RemainingTime = Mathf.Min(RemainingTime + Time.deltaTime, totalTime);
                }
            }
        }
    }

    public void AddTime(float time) {
        SetTime(totalTime + time);
        if (IsCounting) {
            RemainingTime += time;
        }
    }

    public void SetTime(float time) {
        Debug.Assert(time >= 0f);
        totalTime = time;
    }

    public void MinusRemainingTime(float time) {
        if (!IsCounting) {
            Debug.LogWarning("カウント中じゃないから意味ないやで");
        }
        RemainingTime = Mathf.Max(RemainingTime - time, 0);
    }

    public void StartTimer(float? time = null) {
        if (time.HasValue) {
            SetTime(time.Value);
        }
        IsCounting = true;
        RemainingTime = totalTime;
        IsReversed = false;
        timerStartSubject.OnNext(Unit.Default);
    }

    public void ResetTimer() {
        IsCounting = false;
        RemainingTime = 0f;
        IsReversed = false;
    }

    public void Reverse() {
        IsReversed = true;
    }

    public void Normal() {
        IsReversed = false;
    }
}
