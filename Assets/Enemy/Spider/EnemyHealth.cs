using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    public Animator animator;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioClip PoofSound;

    public GameObject explosionPrefab; // 🔥 Reference to explosion prefab

    private bool isDead = false;
    private Rigidbody2D rb;
    private SpiderEnemy spiderAI; // Reference to movement script

    public float knockbackForce = 3f; // Adjust for how much the enemy should be pushed back
    public float hurtDuration = 0.5f; // How long the enemy stays hurt

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
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
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
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        yield return new WaitForSeconds(1f); // Wait for animation to finish

        if (PoofSound != null)
        {
            AudioSource.PlayClipAtPoint(PoofSound, transform.position);
        }

        // Spawn explosion slightly higher to match the enemy's body
        if (explosionPrefab != null)
        {
            Vector3 explosionPosition = transform.position + new Vector3(0, 1.5f, 0); // Adjust Y position
            GameObject explosion = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
            Destroy(explosion, 1f); // Destroy after animation finishes
        }

        GetComponent<Collider2D>().enabled = false; // Disable only before destruction
        Destroy(gameObject);
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        StartCoroutine(DieSequence());
    }
}
