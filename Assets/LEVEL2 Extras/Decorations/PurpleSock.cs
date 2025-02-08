using UnityEngine;
using System.Collections;

public class PurpleSock : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    public Animator animator;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioClip PoofSound;

    public GameObject poofPrefab; // 🔥 Reference to explosion prefab
    public Transform poofPosition; // Assign a specific spawn point if needed
    public bool SockDead = false;
    private Rigidbody2D rb;
    public SpiderEnemy broomEnemy;

    public float knockbackForce = 0.1f; // Adjust for how much the enemy should be pushed back
    public Sprite sockAchievementSprite; // Assign in the Inspector

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); // Ensure this is set
        rb = GetComponent<Rigidbody2D>();

        if (broomEnemy == null)
        {
            Debug.LogError("❌ Could not find SpiderEnemy in the scene!");
        }
    }

    public void TakeDamage(int damage, Vector2 attackSource)
    {
        if (SockDead) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Current Health: " + currentHealth);

        if (damageSound != null)
        {
            AudioManager.instance.PlaySound(damageSound);
        }

        if (animator != null)
        {
            animator.SetTrigger("Hurt"); // Play hurt animation
            Debug.Log("Took damage!");
        }

        if (animator != null)
        {
            animator.SetTrigger("Hurt"); // Play hurt animation
            StartCoroutine(ResetHurtTrigger()); // Reset trigger after short delay
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ResetHurtTrigger()
    {
        yield return new WaitForSeconds(1f); // Wait for hurt animation duration

        animator.ResetTrigger("Hurt");
    }

    IEnumerator DieSequence()
    {
        Debug.Log(gameObject.name + " has died.");

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (deathSound != null)
        {
            AudioManager.instance.PlaySound(deathSound);
        }

        yield return new WaitForSeconds(1f); // Wait for animation to finish

        if (PoofSound != null)
        {
            AudioManager.instance.PlaySound(PoofSound);
        }

        // Spawn explosion slightly higher to match the enemy's body
        if (poofPrefab != null)
        {
            Vector3 explosionPosition = transform.position + new Vector3(0, 1.5f, 0); // Adjust Y position
            GameObject explosion = Instantiate(poofPrefab, explosionPosition, Quaternion.identity);
            Destroy(explosion, 1f); // Destroy after animation finishes
        }

        GetComponent<Collider2D>().enabled = false; // Disable only before destruction
        Destroy(gameObject);

        // 🏆 Trigger Achievement
        AchievementManager.instance.ShowAchievement(sockAchievementSprite);
    }

    void Die()
    {
        if (SockDead) return;
        SockDead = true;

        StartCoroutine(DieSequence());
    }
}
