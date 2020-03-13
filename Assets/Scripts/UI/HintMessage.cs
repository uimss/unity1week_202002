using UnityEngine;
using TMPro;

public class HintMessage : MonoBehaviour {
    [SerializeField] TextMeshProUGUI hintLabel = default;
    [SerializeField] Animator mAnimator = default;

    private int playCountThrehold = 2;

    private readonly string[] messages = new string[] {
        "<!> 春の音は、芽に近づくほど大きく聞こえるようだ",
        "<!> 木のないところには、芽は生えにくいようだ",
        "<!> 芽は、いくつかまとまって生えてるようだ"
    };

    public void TryShowHintMessage() {
        var playCount = GameState.Instance.GetPlayCount();
        if (playCount < playCountThrehold) {
            return;
        }
        var messageIndex = (int)Mathf.Repeat(playCount - playCountThrehold, messages.Length - 1);
        hintLabel.text = messages[messageIndex];
        mAnimator.Play("Show", 0, 0f);
    }
}
