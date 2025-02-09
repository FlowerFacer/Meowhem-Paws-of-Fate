using UnityEngine;

public class BroomAttack : MonoBehaviour
{
    public int damage = 3;
    public Transform attacker; // Assign the player to this in the Inspector

    void Start()
    {
        if (attacker == null)
        {
            attacker = GameObject.FindGameObjectWithTag("Enemy2").transform;
        }
        else
        {
            Debug.Log("Couldn't find the broom!");
        }
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
