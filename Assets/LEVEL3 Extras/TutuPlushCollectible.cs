using UnityEngine;

public class TutuPlushCollectible : MonoBehaviour
{
    public int specialPowerIncreaseAmount = 5; // How much max health to add
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
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // Stops movement but allows destruction
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched the plushie!"); // ✅ Debugging

            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddTutuPlushie(1); // Add the shroom to inventory
                inventory.UpdateGameData(); // ✅ **Force GameData Update**
            }

            // Play sound when collected
            if (shroomSound != null)
            {
                AudioManager.instance.PlaySound(shroomSound);
            }
            else
            {
                Debug.LogWarning("plushie Sound is missing!");
            }

            Debug.Log("plushie collected! SP ATK power Increased.");
            Destroy(gameObject); // Remove the collectible from the scene
            Debug.Log("plushie was successfully destroyed!");
        }
    }
}
