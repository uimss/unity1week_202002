using UnityEngine;
using UnityEngine.UI;

class DebugUI : MonoBehaviour {
    [SerializeField] private Text timerLabel = default;
    [SerializeField] private Text reverseTimerLabel = default;
    [SerializeField] private SpringSandglass springSandglass = default;

    private void Update() {
        timerLabel.text = Mathf.FloorToInt(springSandglass.GetRemainingTime()).ToString();
        reverseTimerLabel.text = Mathf.FloorToInt(springSandglass.GetReverseRemainingTime()).ToString();
    }
}
