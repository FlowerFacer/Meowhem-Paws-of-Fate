using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreenUI : MonoBehaviour
{
    public TMP_Text maxHealthText;
    public TMP_Text regularAttackText;
    public TMP_Text specialAttackText;
    public TMP_Text secretsText;

    public float loadDelay = 8f; // How long the loading screen stays

    private void Start()
    {
        if (GameData.instance != null)
        {
            maxHealthText.text = "Max Health: " + GameData.instance.maxHealth;
            regularAttackText.text = "Attack Power: " + GameData.instance.regularAttackPower;
            specialAttackText.text = "Special Attack Power: " + GameData.instance.specialAttackPower;

            secretsText.text = $"Secrets Found: \n" +
                $"{GameData.instance.yarnCount}/1 Yarn Balls\n" +
                $"{GameData.instance.fishpoleCount}/1 Fishpoles\n" +
                $"{GameData.instance.magicShroomCount}/1 Magic Mushrooms";
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(loadDelay);

        // Load the next scene (Change "NextLevelScene" to your actual scene name)
        SceneManager.LoadScene("NextLevelScene");
    }
}
