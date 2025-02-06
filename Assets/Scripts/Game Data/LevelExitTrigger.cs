using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelExitTrigger : MonoBehaviour
{
    public string loadingSceneName = "LoadingScene"; // Name of the loding scene
    public Image fadeImage; // Assign a UI Image (Black Panel) in Canvas
    public float fadeDuration = 1.5f; // Adjust fade speed

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(FadeAndLoad());
        }
    }

    IEnumerator FadeAndLoad()
    {
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