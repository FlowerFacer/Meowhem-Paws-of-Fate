using UnityEngine;
using System.Collections;

public class SlashEffect : MonoBehaviour
{
    private int attackPower = 3; // Default value (overridden on attack)
    public Transform attacker; // Assign the player to this in the Inspector

    void Start()
    {
        if (attacker == null)
        {
            attacker = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public void SetAttackPower(int newPower)
    {
        attackPower = newPower;
    }

    void OnTriggerStay2D(Collider2D other) // Continuous check
    {
        if (other.CompareTag("Plant"))
        {
            LightBluePlant plantHealth = other.GetComponent<LightBluePlant>();
            if (plantHealth != null)
            {
                Vector2 attackSource = attacker != null ? attacker.position : transform.position;
                plantHealth.TakeDamage(attackPower, attackSource);
            }
        }
        else if (other.CompareTag("Sock"))
        {
            PurpleSock sockHealth = other.GetComponent<PurpleSock>();
            if (sockHealth != null)
            {
                Vector2 attackSource = attacker != null ? attacker.position : transform.position;
                sockHealth.TakeDamage(attackPower, attackSource);
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
                enemyHealth.TakeDamage(attackPower, attackSource);
            }
        }
        else if (other.CompareTag("Enemy2"))
        {
            Level2SpiderHealth enemyHealth = other.GetComponent<Level2SpiderHealth>();
            if (enemyHealth != null)
            {
                Vector2 attackSource = attacker != null ? attacker.position : transform.position;
                enemyHealth.TakeDamage(attackPower, attackSource);
            }
        }
    }
}
