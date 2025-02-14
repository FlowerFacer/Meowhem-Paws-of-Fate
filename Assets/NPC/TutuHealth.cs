using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Needed for UI

public class TutuHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    public Animator animator;
    public AudioClip damageSoundMouse;
    public AudioClip deathSoundMouse;
    public AudioClip PoofSound;

    public GameObject poofPrefab; // 🔥 Reference to explosion prefab
    public Transform poofPosition; // Assign a specific spawn point if needed
    public bool PlantDead = false;
    private Rigidbody2D rb;

    public float knockbackForce = 0.1f; // Adjust for how much the enemy should be pushed back
    public Sprite plantAchievementSprite; // Assign in the Inspector

    // **Health Bar UI**
    public Slider healthBarUI; // Ref to tutus UI slider

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); // Ensure this is set
        rb = GetComponent<Rigidbody2D>();

        // 🩸 **Set UI Health Bar**
        if (healthBarUI != null)
        {
            healthBarUI.maxValue = maxHealth;
            healthBarUI.value = currentHealth;
        }
    }

    public void TakeDamage(int damage, Vector2 attackSource)
    {
        if (PlantDead) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Current Health: " + currentHealth);

        if (damageSoundMouse != null)
        {
            AudioManager.instance.PlaySound(damageSoundMouse);
        }

        if (animator != null)
        {
            animator.SetTrigger("Hurt"); // Play hurt animation
            StartCoroutine(ResetHurtTrigger()); // Reset trigger after short delay
        }

        // **Update UI Health Bar**
        if (healthBarUI != null)
        {
            healthBarUI.value = currentHealth;
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

        if (deathSoundMouse != null)
        {
            AudioManager.instance.PlaySound(deathSoundMouse);
        }

        yield return new WaitForSeconds(2f); // Wait for animation to finish

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

        // 🩸 **Hide UI Health Bar when dead**
        if (healthBarUI != null)
        {
            healthBarUI.gameObject.SetActive(false);
        }

        GetComponent<Collider2D>().enabled = false; // Disable only before destruction
        Destroy(gameObject);

        // 🏆 Trigger Achievement
        AchievementManager.instance.ShowAchievement(plantAchievementSprite);
    }

    void Die()
    {
        if (PlantDead) return;
        PlantDead = true;

        StartCoroutine(DieSequence());
    }
}
