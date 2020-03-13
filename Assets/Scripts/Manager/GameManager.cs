using UnityEngine;
using UniRx.Async;
using UnityEngine.SceneManagement;
using System.Linq;

class GameManager : SingletonMonoBehaviour<GameManager> {
    [SerializeField] private Player player = default;
    [SerializeField] private SpringSandglass springSandglass = default;
    [SerializeField] private ScoreTimeCounter scoreTimeCounter = default;
    [SerializeField] private GameUI gameUI = default;

    private void Start() {
        if (GameState.Instance.IsFirstPlay()) {
            gameUI.OpenTitleMenu();
        } else {
            StartGameSecondary().Forget();
        }
    }

    async private UniTaskVoid StartGameSecondary() {
        await gameUI.FadeOutMask();
        await StartGame();
    }

    async public UniTask StartGame() {
        await UniTask.DelayFrame(5);
        gameUI.ShowSandglass();
        springSandglass.StartTimer();
        scoreTimeCounter.StartCounter();
        player.SetCanAction(true);
    }

    async public UniTask EndGame() {
        scoreTimeCounter.StopCounter();
        var score = CalcGameScore();
        gameUI.HideSandglass();
        await UniTask.WhenAll(
            ShadowManager.Instance.TweenSpring(),
            player.Lost()
        );
        await gameUI.OpenEndMenu(score);
        gameUI.TryShowHintMessage();
    }

    async public UniTask RetryGame() {
        await gameUI.FadeInMask();
        GameState.Instance.AddPlayCount();
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    // TODO ここ以降、GameManagerの役割じゃない気がする
    public Vector3 GetPlayerPosition() {
        return player.transform.position;
    }

    public bool IsNearPlayer(SpringPoint point) {
        return player.IsNearPoint(point);
    }

    private GameScore CalcGameScore() {
        var points = GameObject.FindGameObjectsWithTag(TagName.SpringPoint).Select(obj => obj.GetComponent<SpringPoint>());
        var pickedCount = points.Where(point => point.IsPicked()).Count();
        var remainingCount = points.Count() - pickedCount;
        var time = scoreTimeCounter.ElapsedTime;
        return new GameScore(time, pickedCount, remainingCount);
    }
}

class GameScore {
    public float time;
    public int pickedCount;
    public int remainingCount;

    public GameScore(float _time, int _pickedCount, int _remainingCount) {
        time = _time;
        pickedCount = _pickedCount;
        remainingCount = _remainingCount;
    }
}

