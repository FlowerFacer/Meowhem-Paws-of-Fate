using UnityEngine;

public class SpiderAttackEffect : MonoBehaviour
{
    public int damage = 3;
    public Transform attacker; // Assign the player to this in the Inspector

    void Start()
    {
        attacker = transform; // Spider itself is the attacker
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Vector2 attackSource = attacker != null ? attacker.position : transform.position;
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
