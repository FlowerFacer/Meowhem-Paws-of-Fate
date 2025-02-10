using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class SceneMusicController : MonoBehaviour
{
    [Header("Music Settings")]
    public AudioClip normalMusic; // Default scene music
    public AudioClip bossMusic;   // Boss fight music
    public float normalMusicVolume = 0.5f; // Default volume for normal music
    public float bossMusicVolume = 0.7f;   // Default volume for boss music

    [Header("Lighting Settings")]
    public Light2D globalLight; // Reference to Global Light 2D
    public Color normalLightColor = Color.white; // Default light color
    public Color bossLightColor = Color.red; // Boss fight light color
    public float normalLightIntensity = 1f; // Default light intensity
    public float bossLightIntensity = 0.5f; // Boss fight intensity (darker)

    [Header("Boss Settings")]
    public BossTutu3 bossTutu; // Reference to BossTutu3
    public GameObject tutuBoss; // The actual boss GameObject (Check if destroyed)
    private bool isBigMouseActive = false; // Tracks if boss mode is active

    private AudioSource audioSource;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }

        // Find BossTutu3 in the scene
        if (bossTutu == null)
        {
            bossTutu = FindFirstObjectByType<BossTutu3>();
        }

        // Ensure global light reference exists
        if (globalLight == null)
        {
            globalLight = FindFirstObjectByType<Light2D>();
        }

        PlayNormalMusic();
        ResetLighting(); // Set normal lighting at start
    }

    private void Update()
    {
        if (bossTutu != null)
        {
            if (bossTutu.isBigMouse && !isBigMouseActive)
            {
                isBigMouseActive = true;
                StartCoroutine(FadeToBossMode());
            }
        }

        // 🔥 **Check if boss is defeated (GameObject destroyed)**
        if (tutuBoss == null && isBigMouseActive)
        {
            isBigMouseActive = false;
            StartCoroutine(FadeToNormalMode());
        }
    }

    private void PlayNormalMusic()
    {
        audioSource.clip = normalMusic;
        audioSource.volume = normalMusicVolume;
        audioSource.Play();
    }

    private IEnumerator FadeToBossMode()
    {
        yield return StartCoroutine(FadeMusic(bossMusic, bossMusicVolume));
        yield return StartCoroutine(FadeLighting(bossLightColor, bossLightIntensity));
    }

    private IEnumerator FadeToNormalMode()
    {
        yield return StartCoroutine(FadeMusic(normalMusic, normalMusicVolume));
        yield return StartCoroutine(FadeLighting(normalLightColor, normalLightIntensity));
    }

    private IEnumerator FadeMusic(AudioClip newMusic, float targetVolume)
    {
        float fadeDuration = 1.5f;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newMusic;
        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    private IEnumerator FadeLighting(Color targetColor, float targetIntensity)
    {
        float fadeDuration = 1f;
        Color startColor = globalLight.color;
        float startIntensity = globalLight.intensity;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            globalLight.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            globalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / fadeDuration);
            yield return null;
        }
    }

    private void ResetLighting()
    {
        if (globalLight != null)
        {
            globalLight.color = normalLightColor;
            globalLight.intensity = normalLightIntensity;
        }
    }
}
