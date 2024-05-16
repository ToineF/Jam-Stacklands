using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [field:SerializeField] public AudioData Data { get; private set; }

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _mainMusicSource;
    [SerializeField] private float _mainMusicAudioFadeTime;

    private void Awake()
    {
        Instance = this;
        StartFade(_mainMusicSource, _mainMusicAudioFadeTime, 1);
    }

    public void PlayClip(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}