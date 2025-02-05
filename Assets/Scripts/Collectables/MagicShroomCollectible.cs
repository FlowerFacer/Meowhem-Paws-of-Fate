using UnityEngine;

public class MagicShroomCollectible : MonoBehaviour
{
    public int healthIncreaseAmount = 5; // How much max health to add
    public AudioClip shroomSound; // Optional pickup sound
    private Rigidbody2D rb;
    private bool hasLanded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 🎯 **Stops Mushroom Falling When It Hits the Ground**
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !hasLanded)
        {
            hasLanded = true;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0; // Stops further falling
            rb.bodyType = RigidbodyType2D.Static; // Stops physics movement
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddMagicShroom(1); // Add the shroom to inventory
            }

            // Play sound when collected
            if (shroomSound != null)
            {
                AudioManager.instance.PlaySound(shroomSound);
            }

            Debug.Log("Magic Mushroom collected! Max Health Increased.");
            Destroy(gameObject); // Remove the collectible from the scene
        }
    }
}
