using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Loading2SceneUI : MonoBehaviour
{
    public TMP_Text maxHealthText;
    public TMP_Text regularAttackText;
    public TMP_Text specialAttackText;
    public TMP_Text secretsText;

    public float loadDelay = 13f; // How long the loading screen stays
    public float typeSpeed = 0.03f; // Speed of typewriter effect

    void Start()
    {
        StartCoroutine(TypewriterEffect());
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator TypewriterEffect()
    {
        if (GameData.instance != null)
        {
            yield return StartCoroutine(TypeText(maxHealthText, $"Max HP : {GameData.instance.maxHealth}"));
            yield return StartCoroutine(TypeText(regularAttackText, $"ATK Power : {GameData.instance.regularAttackPower}"));
            yield return StartCoroutine(TypeText(specialAttackText, $"SP ATK Power : {GameData.instance.specialAttackPower}"));
            yield return StartCoroutine(TypeText(secretsText, $"Secrets Found :\n" +
                $"{GameData.instance.yarnCount} / 2 Yarn Balls\n" +
                $"{GameData.instance.fishpoleCount} / 2 Fishpoles\n" +
                $"{GameData.instance.magicShroomCount} / 2 Magic Mushrooms"));
        }
    }

    IEnumerator TypeText(TMP_Text textComponent, string fullText)
    {
        textComponent.text = "";
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(loadDelay);
        // Load the next scene (Change "NextLevelScene" to your actual scene name)
        SceneManager.LoadScene("Level3"); // Loads level2 scene
    }
}
