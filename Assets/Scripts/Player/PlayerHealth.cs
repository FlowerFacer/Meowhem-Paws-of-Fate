using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerController playerController; // Default max health
    public int maxHealth = 10;
    public int currentHealth;
    public Animator animator;
    public AudioClip Bonk;

    void Start()
    {
        playerController = GetComponent<PlayerController>(); // Get the PlayerController script
        currentHealth = maxHealth; // Initialize health
    }

    public void TakeDamage(int amount)
    {
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
    }

    void Die()
    {
        Debug.Log("Player died!");
        animator.SetTrigger("isDead");
        // Handle player death (e.g., restart level)
    }
}
