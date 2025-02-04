using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerController playerController; // Reference to PlayerController
    public int health = 10;
    public Animator animator;

    void Start()
    {
        playerController = GetComponent<PlayerController>(); // Get the PlayerController script
    }

    public void TakeDamage(int amount)
    {
        // Prevent damage if the player is performing a special attack
        if (playerController.isSpecialAttacking)
        {
            Debug.Log("Player is Special Attacking! No damage taken.");
            return; // Skip damage processing
        }

        health -= amount;
        Debug.Log("Player took damage! Health: " + health);
        animator.SetTrigger("HurtTrigger");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        animator.SetTrigger("isDead");
        // Handle player death (e.g., restart level)
    }
}
