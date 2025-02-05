using UnityEngine;

public class YarnballCollectible : MonoBehaviour
{
    public int yarnPoints = 1; // Amount added per yarn ball
    public AudioClip yarnSound;

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
