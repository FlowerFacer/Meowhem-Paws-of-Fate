using UnityEngine;

public class BroomEnemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float patrolDistance = 2.5f;
    public float detectionRange = 4f;
    public int damage = 2;
    public Transform player;
    public Animator animator;

    private Vector3 initialPosition;
    private bool movingRight = true;
    private bool playerDetected = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (playerDetected)
        {
            AttackPlayer();
        }
        else
        {
            Patrol();
            DetectPlayer();
        }
    }

    void Patrol()
    {
        float patrolLimit = movingRight ? initialPosition.x + patrolDistance : initialPosition.x - patrolDistance;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(patrolLimit, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - patrolLimit) < 0.1f)
        {
            movingRight = !movingRight;
            Flip();
        }
    }

    void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            playerDetected = true;
            animator.SetTrigger("SweepAttack");
        }
    }

    void AttackPlayer()
    {
        // Charge toward the player
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * 1.5f * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.position) < 1.5f)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
