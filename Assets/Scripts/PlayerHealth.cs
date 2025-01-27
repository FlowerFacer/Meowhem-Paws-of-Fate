using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 10;
    public Animator animator;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Player took damage! Health: " + health);

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
