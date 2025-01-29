using UnityEngine;

public class YarnballCollectible : MonoBehaviour
{
    public int yarnPoints = 1; // Amount added per yarn ball

    public AudioClip yarnSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddYarn(yarnPoints);
            }

            // Play sound when collected
            if (yarnSound != null)
            {
                AudioManager.instance.PlaySound(yarnSound);
            }
            else
            {
                Debug.LogWarning("Yarn Sound is missing!");
            }

            Debug.Log("Yarn Ball collected! Total yarn count increased.");
            Destroy(gameObject); // Remove the collectible from the scene
        }
    }
}
