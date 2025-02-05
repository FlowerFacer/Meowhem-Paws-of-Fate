using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;

    public GameObject achievementPanel;
    public Image achievementImage; // Only using an image now
    public AudioClip achievementSound;
    private AudioSource audioSource;

    private CanvasGroup canvasGroup; // To control fade-out

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (achievementPanel != null)
        {
            achievementPanel.SetActive(false); // Hide achievement UI at start
            canvasGroup = achievementPanel.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = achievementPanel.AddComponent<CanvasGroup>(); // Add if missing
            }

            canvasGroup.alpha = 0; // Start fully transparent
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void ShowAchievement(Sprite achievementSprite)
    {
        if (achievementPanel != null)
        {
            achievementImage.sprite = achievementSprite; // Set the correct achievement image
            achievementPanel.SetActive(true);

            if (achievementSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(achievementSound);
            }

            StartCoroutine(FadeInAndFadeOut());
        }
    }

    IEnumerator FadeInAndFadeOut()
    {
        // **Fade In**
        float fadeTime = 0;
        float fadeDuration = 0.8f; // How long to fade in

        while (fadeTime < fadeDuration)
        {
            fadeTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, fadeTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1; // Ensure fully visible
        yield return new WaitForSeconds(3f); // Display time

        // **Fade Out**
        fadeTime = 0;
        float fadeOutDuration = 1f; // How long to fade out

        while (fadeTime < fadeOutDuration)
        {
            fadeTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, fadeTime / fadeOutDuration);
            yield return null;
        }

        // **Hide after fading out**
        achievementPanel.SetActive(false);
    }
}
