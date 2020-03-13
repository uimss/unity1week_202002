using UnityEngine;
using System.Linq;
using DG.Tweening;
using UniRx;
using UniRx.Async;
using System.Collections.Generic;
using System;

class Player : MonoBehaviour {

    [SerializeField] private Collider2D findArea = default;
    [SerializeField] private SpringSandglass springSandglass = default;
    [SerializeField] private Footprinter footprinter = default;
    [SerializeField] private EmptyPointPrinter emptyPointPrinter = default;
    [SerializeField] private Animator mAnimator = default;
    [SerializeField] private SpriteRenderer mRenderer = default;
    [SerializeField] private BoxCollider2D shadowCollider = default;
    [SerializeField] private Direction2D4Animation walkAnim = default;
    [SerializeField] private Direction2D4Animation idleAnim = default;
    [SerializeField] private string lostAnim = default;
    [SerializeField] private AudioClip lostSe = default;
    private const float moveSpeed = 40f;
    private const float findDirectionOffset = 15f;
    // TODO あとで見直したい〜〜〜
    private bool canAction = false;
    private bool canPick = true;
    private Vector2 direction = Vector2.up;
    private Vector2 footDirection = Vector2.up;
    private string currentAnim = default;
    private Vector3 findAreaLocalPos = default;

    private void Start() {
        transform.position = StageUtil.UprightZPosition(transform.position);
        currentAnim = GetAnimStateName(direction.ToDirection4(), idleAnim);
        findAreaLocalPos = findArea.transform.localPosition;
    }

    private void Update() {
        if (canAction) {
            var moveX = Input.GetAxis("Horizontal");
            var moveY = Input.GetAxis("Vertical");
            var move2D = new Vector2(moveX, moveY);
            if (move2D != Vector2.zero) {
                Walk(move2D);
            } else {
                Idle();
            }
            if (canPick && Input.GetButtonDown("Jump")) {
                TryPick().Forget();
            }
        }
    }

    private void Walk(Vector2 move2D) {
        // 入力値の補正
        var moveX = move2D.x != 0 ? Mathf.Sign(move2D.x) * Mathf.Clamp(Mathf.Abs(move2D.x), 0.4f, 1f) : 0;
        var moveY = move2D.y != 0 ? Mathf.Sign(move2D.y) * Mathf.Clamp(Mathf.Abs(move2D.y), 0.4f, 1f) : 0;
        var pos = Vector3.MoveTowards(transform.position, transform.position.Add(x: moveX, y: moveY), moveSpeed * Time.deltaTime);

        // 上方向を若干強めに評価する
        direction = move2D.Overwrite(y: move2D.y * 1.1f).normalized;
        footDirection = transform.position.Direction(pos);

        // animation
        TryPlayAnim(walkAnim);
        // position
        pos = StageUtil.UprightZPosition(pos);
        transform.position = pos;
        // findarea position
        findArea.transform.localPosition = findAreaLocalPos + ((Vector3)direction * findDirectionOffset);
    }

    private void Idle() {
        var onPlay = TryPlayAnim(idleAnim);
        if (onPlay) {
            OnFootstepStop();
        }
    }

    async private UniTaskVoid TryPick() {
        canPick = false;
        var springPoint = FindNearPoint();
        if (springPoint != null) {
            var success = springPoint.TryPick();
            if (success) {
                springSandglass.Reverse();
            }
        } else {
            emptyPointPrinter.Generate(findArea.transform.position);
            springSandglass.Damage().Forget();
        }
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
        canPick = true;
    }

    public bool IsNearPoint(SpringPoint point) {
        var springPoint = FindNearPoint();
        return springPoint != null && springPoint == point;
    }

    private SpringPoint FindNearPoint() {
        var filter = new ContactFilter2D();
        var results = new Collider2D[10];
        filter.useTriggers = true;
        filter.SetLayerMask(LayerName.SpringPointMask);

        findArea.OverlapCollider(filter, results);
        return results
            .Where(c => c != null)
            .Select(c => c.GetComponent<SpringPoint>())
            .FirstOrDefault();
    }

    private void OnFootstepStop() {
        footprinter.GenerateSilent(transform.position, footDirection.ToDirectionDegree(), true);
        footprinter.GenerateSilent(transform.position, footDirection.ToDirectionDegree(), false);
    }

    private void OnFootstepL() {
        footprinter.Generate(transform.position, footDirection.ToDirectionDegree(), true);
    }

    private void OnFootstepR() {
        footprinter.Generate(transform.position, footDirection.ToDirectionDegree(), false);
    }

    public void SetCanAction(bool newCanAction) {
        canAction = newCanAction;
    }

    async public UniTask Lost() {
        canAction = false;
        SoundManager.Instance.PlaySe(lostSe);
        await DOTween
            .To(
                value => mRenderer.material.SetColor("_AddColor", new Color(value, value, value)),
                0f, 1f, 1f
            )
            .SetEase(Ease.Linear)
            .ToAwaiter();
        shadowCollider.size = new Vector2(5f, 5f);
        mRenderer.material.SetColor("_AddColor", Color.black);
        mAnimator.Play(lostAnim);
    }

    private bool TryPlayAnim(Direction2D4Animation animationMap) {
        var anim = GetAnimStateName(direction.ToDirection4(), animationMap);
        if (currentAnim != anim) {
            mAnimator.Play(anim);
            currentAnim = anim;
            return true;
        }
        return false;
    }

    private string GetAnimStateName(Direction2D4 direction, Direction2D4Animation animationMap) {
        var dict = new Dictionary<Direction2D4, string>() {
            {Direction2D4.Top, animationMap.Top},
            {Direction2D4.Left, animationMap.Left},
            {Direction2D4.Bottom, animationMap.Bottom},
            {Direction2D4.Right, animationMap.Right},
        };
        return dict[direction];
    }
}

[System.Serializable]
class Direction2D4Animation {
    public string Top = default;
    public string Left = default;
    public string Bottom = default;
    public string Right = default;
}
