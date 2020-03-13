using UnityEngine;
using System.Linq;
using DG.Tweening;

public class EmptyPointPrinter : MonoBehaviour {
    [SerializeField] private GameObject emptyPointPrefab = default;
    [SerializeField] private AudioClip diggSe = default;
    private GameObject[] points = new GameObject[0];
    private const int maxPoints = 100;

    public void Generate(Vector2 position) {
        if (points.Length >= maxPoints) {
            RemoveOld();
        }

        var newPoint = Instantiate(emptyPointPrefab, position, Quaternion.identity);
        points = points.Append(newPoint).ToArray();

        PlayDiggSnowParticle(position);
        SoundManager.Instance.PlaySe(diggSe);
    }

    private void PlayDiggSnowParticle(Vector2 position) {
        var point = StageUtil.UprightZPosition(position);
        CommonResource.Instance.diggSnowParticle.transform.position = point;
        CommonResource.Instance.diggSnowParticle.Play();
    }

    private void RemoveOld() {
        var oldPoint = points.First();

        points = points.Skip(1).ToArray();
        oldPoint.GetComponent<EmptyPoint>().Remove();
    }
}
