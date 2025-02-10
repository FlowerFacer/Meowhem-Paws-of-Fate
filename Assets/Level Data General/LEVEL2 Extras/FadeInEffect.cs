using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInEffect : MonoBehaviour
{
    public Image fadePanel; // Assign a UI Image (Black Panel) in Canvas
    public float fadeDuration = 1.5f; // Adjust fade speed

    void Start()
    {
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color fadeColor = fadePanel.color;
        fadeColor.a = 1f; // Start fully black

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeColor.a = Mathf.Clamp01(1 - (elapsedTime / fadeDuration)); // Fade to transparent
            fadePanel.color = fadeColor;
            yield return null;
        }

        fadePanel.gameObject.SetActive(false); // Disable once fully faded
    }
}
