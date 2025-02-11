using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreditsRoll : MonoBehaviour
{
    public float scrollSpeed = 50f; // Adjust to control speed
    private RectTransform rectTransform;
    private float startY;
    private float endY;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startY = rectTransform.anchoredPosition.y;
        endY = startY + rectTransform.rect.height; // End point
        StartCoroutine(ScrollCredits());
    }

    IEnumerator ScrollCredits()
    {
        while (rectTransform.anchoredPosition.y < endY)
        {
            rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
            yield return null;
        }

        // Optionally restart or load a new scene
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MenuScene");
    }
}
