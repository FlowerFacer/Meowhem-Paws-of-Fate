using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Level4ExitTrigger : MonoBehaviour
{
    public string loadingSceneName = "Credits"; // Name of the loading scene
    public Image fadeImage; // Assign a UI Image (Black Panel) in Canvas
    public float fadeDuration = 1.5f; // Adjust fade speed

    private bool hasTriggered = false;
    private bool playerInside = false; // Track if player is inside trigger

    public Animator anim;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true; // Player is inside trigger
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false; // Player left trigger
        }
    }

    private void Update()
    {
        // Check if player is inside and presses E
        if (playerInside && Input.GetKeyDown(KeyCode.E) && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(FadeAndLoad());
        }
    }

    IEnumerator FadeAndLoad()
    {
        anim.SetBool("isSleep", true);

        // Ensure fade image is active
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            Color fadeColor = fadeImage.color;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                fadeColor.a = Mathf.Clamp01(elapsedTime / fadeDuration); // Fade to black
                fadeImage.color = fadeColor;
                yield return null;
            }
        }

        // ✅ Load Next Scene
        SceneManager.LoadScene(loadingSceneName);
    }
}
