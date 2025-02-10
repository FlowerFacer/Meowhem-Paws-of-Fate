using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private PlayerController playerController; // Default max health
    public int maxHealth = 10;
    public int currentHealth;
    public Animator animator;
    public AudioClip Bonk;
    public bool isDead = false;

    void Start()
    {
        //if (GameData.instance != null)
        //{
        //    maxHealth = GameData.instance.maxHealth; // ✅ Load saved HP
        //    currentHealth = maxHealth; // Set starting HP
        //    Debug.Log("📥 Loaded Max HP: " + maxHealth);
        //}

        playerController = GetComponent<PlayerController>(); // Get the PlayerController script
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

        StopAllCoroutines(); // **Stop any active patrol or attack coroutine**
    }

    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(2.5f); // Adjust timing as needed
        Debug.Log("Respawning or Restarting Level...");
        // Handle respawn or restart logic here
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
