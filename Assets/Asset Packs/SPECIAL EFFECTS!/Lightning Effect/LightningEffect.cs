using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    public int damage = 8;
    public Transform attacker; // Assign the player to this in the Inspector

    void Start()
    {
        if (attacker == null)
        {
            attacker = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                Vector2 attackSource = attacker != null ? attacker.position : transform.position;
                enemyHealth.TakeDamage(damage, attackSource); // Call the damage method
            }
        }
    }
}
