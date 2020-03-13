using UnityEngine;

public class ScoreTimeCounter : MonoBehaviour {
    public float ElapsedTime { get; private set; } = 0f;
    public bool IsCounting { get; private set; } = false;

    private void Update() {
        if (IsCounting) {
            ElapsedTime = ElapsedTime + Time.deltaTime;
        }
    }

    public void StartCounter() {
        ElapsedTime = 0f;
        IsCounting = true;
    }

    public void StopCounter() {
        IsCounting = false;
    }

}
