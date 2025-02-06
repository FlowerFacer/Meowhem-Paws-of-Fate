using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance; // Singleton instance

    // Player Stats
    public int maxHealth = 10; // Default health (Update if it increases)
    public int regularAttackPower = 3; // Matches SlashEffect damage
    public int specialAttackPower = 8; // Matches LightningEffect damage

    // Secrets Collected
    public int yarnCount = 0;
    public int fishpoleCount = 0;
    public int magicShroomCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep data across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateStats(PlayerInventory inventory, SlashEffect slash, LightningEffect lightning)
    {
        // Secrets
        yarnCount = inventory.yarnCount;
        fishpoleCount = inventory.fishpoleCount;
        magicShroomCount = inventory.magicShroomCount;

        // Attack Power
        regularAttackPower = slash.damage;
        specialAttackPower = lightning.damage;

        // Max Health (if you track it elsewhere, update it here)
    }
}
