using UnityEngine;

public class MagicShroomCollectible : MonoBehaviour
{
    public int healthIncreaseAmount = 5; // How much max health to add
    public AudioClip shroomSound; // Optional pickup sound

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
