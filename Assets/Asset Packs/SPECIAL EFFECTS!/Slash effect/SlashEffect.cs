using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    public int damage = 3;
    public Transform attacker; // Assign the player to this in the Inspector

    void Start()
    {
        if (attacker == null)
        {
            attacker = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void OnTriggerStay2D(Collider2D other) // Continuous check
    {
        if (other.CompareTag("Plant"))
        {
            LightBluePlant plantHealth = other.GetComponent<LightBluePlant>();
            if (plantHealth != null)
            {
                Vector2 attackSource = attacker != null ? attacker.position : transform.position;
                plantHealth.TakeDamage(damage, attackSource);
            }
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
                enemyHealth.TakeDamage(damage, attackSource);
            }
      
        }
    }
}
