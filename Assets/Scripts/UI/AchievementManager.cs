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
            achievementPanel.SetActive(false); // Hide achievement UI at start

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

            StartCoroutine(HideAchievementAfterDelay());
        }
    }

    IEnumerator HideAchievementAfterDelay()
    {
        yield return new WaitForSeconds(3f); // Show achievement for 3 seconds
        achievementPanel.SetActive(false);
    }
}
