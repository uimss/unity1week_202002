using UnityEngine;
using System.Linq;

class SoundManager : SingletonMonoBehaviour<SoundManager> {

    private const int cNumChannel = 32;
    private AudioSource bgmSource;
    private AudioSource[] seSources = new AudioSource[cNumChannel];

    override protected void Awake() {
        base.Awake();
        DontDestroyOnLoad(this);

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

        for (int i = 0; i < seSources.Length; i++) {
            seSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public AudioSource FindEmptySeSource() {
        return seSources.FirstOrDefault(source => !source.isPlaying);
    }

    public AudioSource PlaySe(AudioClip clip, float volume = 1f, float pitch = 1f) {
        var source = FindEmptySeSource();
        if (!source) {
            Debug.LogWarning("empty Se source not found.");
            return null;
        }

        return PlaySe(clip, source, volume, pitch);
    }

    public AudioSource PlaySe(AudioClip clip, AudioSource source, float volume = 1f, float pitch = 1f) {
        source.Stop();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        return source;
    }

    public AudioSource GetBgmSource() {
        return bgmSource;
    }

    public AudioSource PlayBgm(AudioClip clip, float volume = 1f) {
        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.Play();

        return bgmSource;
    }

    public void StopBgm() {
        bgmSource.Stop();
        bgmSource.clip = null;
    }

    public void StopAllSe() {
        foreach (AudioSource source in seSources) {
            source.Stop();
            source.clip = null;
        }
    }

    public void StopSe(AudioClip clip) {
        foreach (AudioSource source in seSources) {
            if (source.isPlaying && source.clip == clip) {
                source.Stop();
                source.clip = null;
            }
        }
    }
}
