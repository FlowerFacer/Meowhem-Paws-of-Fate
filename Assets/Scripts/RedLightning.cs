using UnityEngine;

public class RedLightning : MonoBehaviour
{
    public int damage = 4; // Adjust damage amount
    public float attackDuration = 1f; // How long the attack stays active before disappearing

    private bool hasDamagedPlayer = false; // Prevents multiple hits

    void Start()
    {
        Destroy(gameObject, attackDuration); // Auto-destroy attack prefab
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasDamagedPlayer)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                hasDamagedPlayer = true; // Prevent multiple hits
            }
        }
    }
}
