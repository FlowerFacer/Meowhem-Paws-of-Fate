using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    public int yarnCount = 0; // Track the number of yarn balls collected
    public int fishpoleCount = 0; // Track the number of yarn balls collected
    public int magicShroomCount = 0; // Track the number of magic mushrooms collected
    public TMP_Text HpIncreaseText; // 🗨️ Reference to TextMeshPro 3D
    public AudioClip increase;

    private PlayerHealth playerHealth; // Reference to player's health system

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>(); // Get reference to PlayerHealth script
    }

    public void AddYarn(int amount)
    {
        yarnCount += amount;
        Debug.Log("Current Yarn Balls: " + yarnCount);
    }
    public void AddFishpole(int amount)
    {
        fishpoleCount += amount;
        Debug.Log("Current Yarn Balls: " + fishpoleCount);
    }

    public void AddMagicShroom(int amount)
    {
        magicShroomCount += amount;
        Debug.Log("Magic Mushroom Collected! Total: " + magicShroomCount);

        ShowHealthBonusText();

        // Increase Player's Max Health
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(5);
        }
    }

    IEnumerator ShowHealthBonusText()
    {
        string text = "+ HP increased by 5!";
        HpIncreaseText.text = "";

        if (increase != null)
        {
            AudioManager.instance.PlaySound(increase);
        }

        foreach (char letter in text) { HpIncreaseText.text += letter; yield return new WaitForSeconds(0.04f); }
    }
}
