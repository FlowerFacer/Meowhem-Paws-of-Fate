using System.Collections;
using TMPro;
using UnityEngine;

public class LoadingDots : MonoBehaviour
{
    public TMP_Text loadingText;
    public float typeSpeed = 0.3f; // Speed of dots appearing

    private void Start()
    {
        StartCoroutine(AnimateLoadingText());
    }

    IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            loadingText.text = "Loading";
            yield return new WaitForSeconds(typeSpeed);
            loadingText.text = "Loading.";
            yield return new WaitForSeconds(typeSpeed);
            loadingText.text = "Loading..";
            yield return new WaitForSeconds(typeSpeed);
            loadingText.text = "Loading...";
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
