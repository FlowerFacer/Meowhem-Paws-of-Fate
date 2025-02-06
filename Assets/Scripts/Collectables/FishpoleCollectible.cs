using UnityEngine;

public class FishpoleCollectible : MonoBehaviour
{
    public int fishpolePoints = 1; // Amount added per fishpole
    public AudioClip fishpoleSound;

    // Hover effect variables
    private float hoverSpeed = 2f;
    private float hoverAmount = 0.1f;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Apply hover movement
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddFishpole(1);
                inventory.UpdateGameData(); // ✅ **Force GameData Update**
            }

            // Play sound when collected
            if (fishpoleSound != null)
            {
                AudioManager.instance.PlaySound(fishpoleSound);
            }
            else
            {
                Debug.LogWarning("Fishpole Sound is missing!");
            }

            Debug.Log("Fishpole collected! Total fishpole count increased.");
            Destroy(gameObject); // Remove the collectible from the scene
        }
    }
}
