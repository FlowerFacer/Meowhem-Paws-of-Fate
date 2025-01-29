using UnityEngine;

public class FishpoleCollectible : MonoBehaviour
{
    public int fishpolePoints = 1; // Amount added per yarn ball

    public AudioClip fishpoleSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddFishpole(fishpolePoints);
            }

            // Play sound when collected
            if (fishpoleSound != null)
            {
                AudioManager.instance.PlaySound(fishpoleSound);
            }
            else
            {
                Debug.LogWarning("Yarn Sound is missing!");
            }

            Debug.Log("Fishpole collected! Total fishpole count increased.");
            Destroy(gameObject); // Remove the collectible from the scene
        }
    }
}
