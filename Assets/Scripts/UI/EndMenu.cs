using System;
using UnityEngine;
using TMPro;
using UniRx;
using UniRx.Async;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

class EndMenu : MonoBehaviour {
    [SerializeField] RectTransform menu = default;
    [SerializeField] RectTransform buttonPanel = default;
    [SerializeField] TextMeshProUGUI scoreTimeLabel = default;
    [SerializeField] Button tweetButton = default;
    [SerializeField] Button rankingButton = default;
    [SerializeField] Button retryButton = default;

    // TODO あとで見直したい〜〜〜
    private bool canAction = false;
    private GameScore score = default;

    private void Start() {
        tweetButton.OnClickAsObservable()
            .Subscribe(_ => OnClickTweetButton())
            .AddTo(this.gameObject);
        rankingButton.OnClickAsObservable()
            .Subscribe(_ => OnClickRankingButton())
            .AddTo(this.gameObject);
        retryButton.OnClickAsObservable()
            .Subscribe(_ => OnClickRetryButton())
            .AddTo(this.gameObject);
    }

    async public UniTask Open(GameScore newScore) {
        score = newScore;
        scoreTimeLabel.text = $"きみは {GetViewScoreTime()}秒 春をおくらせた\n芽を{score.pickedCount}個むしり、{score.remainingCount}個のこした";
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        menu.gameObject.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        scoreTimeLabel.gameObject.SetActive(true);
        await DOTween
            .To(
                () => scoreTimeLabel.rectTransform.localPosition.Add(y: 5f),
                value => scoreTimeLabel.rectTransform.localPosition = value,
                scoreTimeLabel.rectTransform.localPosition,
                1f
            )
            .ToAwaiter();
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        buttonPanel.gameObject.SetActive(true);
        canAction = true;
    }

    public void CloseToRetryGame() {
        canAction = false;
        menu.gameObject.SetActive(false);
        GameManager.Instance.RetryGame().Forget();
    }

    private int GetViewScoreTime() {
        return Mathf.FloorToInt(score.time);
    }

    private void OnClickTweetButton() {
        Debug.Log("tweet");
        naichilab.UnityRoomTweet.Tweet(
            "spring_is_not_comming",
            $"{GetViewScoreTime()}秒 春をおくらせた",
            "spring_is_not_comming",
            "unity1week"
        );
    }

    private void OnClickRankingButton() {
        Debug.Log("ranking");
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score.time);
    }

    private void OnClickRetryButton() {
        if (canAction) {
            CloseToRetryGame();
        }
    }
}
