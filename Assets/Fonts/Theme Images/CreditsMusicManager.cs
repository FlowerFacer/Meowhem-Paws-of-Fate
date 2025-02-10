using UnityEngine;
using System.Collections;

public class CreditsMusicManager : MonoBehaviour
{
    public AudioClip normalMusic; // Default scene music
    public AudioClip bossMusic;   // Boss fight music

    public float normalMusicVolume = 0.5f; // Default volume for normal music
    public float bossMusicVolume = 0.7f;   // Default volume for boss music

    private AudioSource audioSource;
    private bool isBigMouse = false; // Tracks transformation state

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }

        PlayNormalMusic();
    }

    private void Update()
    {
        if (isBigMouse && audioSource.clip != bossMusic)
        {
            StartCoroutine(FadeToBossMusic());
        }
    }

    public void SetBigMouseMode(bool state)
    {
        isBigMouse = state;
    }

    public void PlayNormalMusic()
    {
        audioSource.clip = normalMusic;
        audioSource.volume = normalMusicVolume;
        audioSource.Play();
    }

    private IEnumerator FadeToBossMusic()
    {
        float fadeDuration = 1.5f; // Time to fade between tracks
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = bossMusic;
        audioSource.volume = 0;
        audioSource.Play();

        float targetVolume = bossMusicVolume;
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
