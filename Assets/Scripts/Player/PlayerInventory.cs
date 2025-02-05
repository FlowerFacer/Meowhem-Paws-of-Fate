using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int yarnCount = 0; // Track the number of yarn balls collected
    public int fishpoleCount = 0; // Track the number of yarn balls collected
    public int magicShroomCount = 0; // Track the number of magic mushrooms collected

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

        // Increase Player's Max Health
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(5);
        }
    }
}
