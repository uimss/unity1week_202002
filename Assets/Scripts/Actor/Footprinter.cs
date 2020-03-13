using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Footprinter : MonoBehaviour {
    [SerializeField] private GameObject footprintPrefab = default;
    [SerializeField] private Vector2 offset = default;
    [SerializeField] private AudioClip[] footStepSe = default;
    [SerializeField] private Transform snowParticleHolder = default;
    [SerializeField] private ParticleSystem snowParticle = default;
    private GameObject[] footprints = new GameObject[0];
    private const int maxFootprints = 300;

    private void Start() {
        var renderer = footprintPrefab.GetComponent<SpriteRenderer>();
        renderer.color = ShadowManager.Instance.GetColor();
    }

    public void GenerateSilent(Vector2 position, float degree, bool flip) {
        Generate(position, degree, flip, false);
    }

    public void Generate(Vector2 position, float degree, bool flip, bool playEffect = true) {
        if (footprints.Length >= maxFootprints) {
            RemoveOld();
        }

        // 地面の通常オブジェクトよりは下にする
        var footprintPosition = position + offset;
        var rotation = Quaternion.Euler(0, 0, degree);

        var newFootprint = Instantiate(footprintPrefab, footprintPosition, rotation);
        footprints = footprints.Append(newFootprint).ToArray();

        if (flip) {
            newFootprint.GetComponent<SpriteRenderer>().flipX = true;
        }
        PlaySnowParticle(degree);
        if (playEffect) {
            PlayFootstepSe();
        }
    }

    private void PlaySnowParticle(float degree) {
        // TODO オイラー角なんもわからん...
        snowParticleHolder.transform.rotation = Quaternion.Euler(0f, 0f, degree);
        snowParticle.Play();
    }

    private void PlayFootstepSe() {
        if (footStepSe.Length == 0) {
            Debug.LogError("footStepSe is required");
            return;
        }

        var index = Random.Range(0, footStepSe.Length - 1);
        SoundManager.Instance.PlaySe(footStepSe[index], 0.8f, Random.Range(0.8f, 1.2f));
    }

    private void RemoveOld() {
        var oldFootprint = footprints.First();

        footprints = footprints.Skip(1).ToArray();
        oldFootprint.GetComponent<SpriteRenderer>()
            .DOFade(0, 1f)
            .OnComplete(() => Destroy(oldFootprint.gameObject));
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(offset, 1f);
    }
}
