using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private PlayerController playerController; // Default max health
    public int maxHealth = 10;
    public int currentHealth;
    public Animator animator;
    public AudioClip Bonk;
    public AudioClip PlayerDeath;
    public bool isDead = false;

    // 🎨 **Health Bar UI**
    public Slider healthBarUI;

    void Start()
    {
        if (GameData.instance != null)
        {
            maxHealth = GameData.instance.maxHealth; // ✅ Load saved HP
            currentHealth = maxHealth; // Set starting HP
            Debug.Log("📥 Loaded Max HP: " + maxHealth);
        }

        playerController = GetComponent<PlayerController>(); // Get the PlayerController script

        // 🩸 **Set UI Health Bar**
        if (healthBarUI != null)
        {
            healthBarUI.maxValue = maxHealth;
            healthBarUI.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; // ✅ Prevent taking damage when dead
        // Prevent damage if the player is performing a special attack
        if (playerController.isSpecialAttacking)
        {
            Debug.Log("Player is Special Attacking! No damage taken.");
            return; // Skip damage processing
        }

        if (Bonk != null)
        {
            AudioManager.instance.PlaySound(Bonk);
        }

        currentHealth -= amount;
        Debug.Log("Player took damage! Health: " + currentHealth);
        animator.SetTrigger("HurtTrigger");

        // 🩸 **Update UI Health Bar**
        if (healthBarUI != null)
        {
            healthBarUI.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount; // Increase the player's max health
        currentHealth = maxHealth; // Fully heal when gaining max health
        Debug.Log("Max Health Increased! New Max Health: " + maxHealth);
        UpdateGameData(); // ✅ **Force GameData Update**
    }

    void Die()
    {
        if (isDead) return; // ✅ Prevent multiple death calls
        isDead = true;

        Debug.Log("Player died!");
        animator.SetBool("isDead", true); // ✅ Use Bool instead of Trigger

        if (PlayerDeath != null)
        {
            AudioManager.instance.PlaySound(PlayerDeath);
        }

        // 🩸 **Hide UI Health Bar when dead**
        if (healthBarUI != null)
        {
            healthBarUI.gameObject.SetActive(false);
        }

        // ❌ Disable PlayerController so movement is disabled
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // 🛑 Freeze Rigidbody to prevent sliding
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Stop movement
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // ✅ Completely freeze physics
        }

        StopAllCoroutines(); // **Stop any active patrol or attack coroutine**

        StartCoroutine(HandleDeath());
    }

    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(4f); // Adjust timing as needed
        Debug.Log("Respawning or Restarting Level...");

        // ✅ Find DeathMenu dynamically
        DeathMenu deathMenu = FindFirstObjectByType<DeathMenu>();
        if (deathMenu != null)
        {
            deathMenu.ShowDeathMenu();
        }
    }

    public void UpdateGameData()
    {
        if (GameData.instance != null)
        {
            PlayerController damage = GetComponent<PlayerController>();
            PlayerInventory inventory = GetComponent<PlayerInventory>();
            PlayerHealth health = GetComponent<PlayerHealth>();

            if (damage != null && inventory != null)
            {
                GameData.instance.UpdateStats(inventory, damage, health);
            }
        }
    }
}
