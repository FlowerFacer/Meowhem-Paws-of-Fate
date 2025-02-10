using UnityEngine;
using System.Collections;

public class SceneMusicController : MonoBehaviour
{
    public AudioClip normalMusic; // Default scene music
    public AudioClip bossMusic;   // Boss fight music

    public float normalMusicVolume = 0.5f; // Default volume for normal music
    public float bossMusicVolume = 0.7f;   // Default volume for boss music

    private AudioSource audioSource;
    private BossTutu3 bossTutu; // Reference to BossTutu3
    private bool isBigMouseActive = false; // Tracks if boss mode is active

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }

        // Find BossTutu3 in the scene
        bossTutu = FindFirstObjectByType<BossTutu3>();

        PlayNormalMusic();
    }

    private void Update()
    {
        if (bossTutu != null)
        {
            // If Tutu transforms into BigMouse, switch music
            if (bossTutu.isBigMouse && !isBigMouseActive)
            {
                isBigMouseActive = true;
                StartCoroutine(FadeToBossMusic());
            }
        }
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
