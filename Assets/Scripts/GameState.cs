using UnityEngine;

class GameState : SingletonMonoBehaviour<GameState> {
    // 何回目のプレイか
    private int playCount = 1;

    override protected void Awake() {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public bool IsFirstPlay() {
        return playCount == 1;
    }

    public void AddPlayCount() {
        playCount += 1;
    }

    public int GetPlayCount() {
        return playCount;
    }
}
