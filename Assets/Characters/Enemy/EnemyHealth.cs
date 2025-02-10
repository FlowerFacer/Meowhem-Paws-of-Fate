using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 9;
    private int currentHealth;

    public Animator animator;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioClip PoofSound;

    public GameObject explosionPrefab; // 🔥 Reference to explosion prefab

    public bool isDead = false;
    private Rigidbody2D rb;
    private SpiderEnemy spiderAI; // Reference to movement script

    public float knockbackForce = 6f; // Adjust for how much the enemy should be pushed back
    public float hurtDuration = 0.7f; // How long the enemy stays hurt
    public Sprite spiderAchievementSprite; // Assign in the Inspector

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Ensure this is set
        spiderAI = GetComponent<SpiderEnemy>(); // Get reference to movement script
    }

    public void TakeDamage(int damage, Vector2 attackSource)
    {
        if (isDead) return;

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

        if (spiderAI != null)
        {
            spiderAI.enabled = false; // Temporarily disable movement
        }

        // Knockback effect
        Vector2 knockbackDirection = (transform.position - (Vector3)attackSource).normalized;
        rb.linearVelocity = knockbackDirection * knockbackForce;

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
        yield return new WaitForSeconds(hurtDuration); // Wait for hurt animation duration

        animator.ResetTrigger("Hurt");

        if (!isDead && spiderAI != null)
        {
            spiderAI.enabled = true; // Re-enable movement after recovering
        }
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

        if (explosionPrefab != null)
        {
            // Keep Y as the prefab's original Y position
            Vector3 explosionPosition = new Vector3(transform.position.x, explosionPrefab.transform.position.y, transform.position.z);

            GameObject explosion = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
            Destroy(explosion, 1f); // Destroy after animation finishes
        }

        BroomEnemy broomEnemy = GetComponent<BroomEnemy>(); // Get reference to BroomEnemy script

        if (broomEnemy != null)
        {
            broomEnemy.StopAngryRant(); // Stop the ranting when the broom dies
        }

        GetComponent<Collider2D>().enabled = false; // Disable only before destruction
        Destroy(gameObject);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        StartCoroutine(DieSequence());

        // 🏆 Trigger Achievement
        AchievementManager.instance.ShowAchievement(spiderAchievementSprite);
    }
}
