using UnityEngine;

public class BroomEnemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float patrolDistance = 2.5f;
    public float detectionRange = 4f;
    public int damage = 2;
    public Transform Player1;
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

            if (!movingRight)
            {
                Flip();  // Flip only when moving left
            }
            else
            {
                ResetFlip();  // Reset flip when moving right
            }
        }
    }

    void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, Player1.position) < detectionRange)
        {
            playerDetected = true;
            animator.SetTrigger("SweepAttack");
        }
    }

    void AttackPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, Player1.position, moveSpeed * 1.5f * Time.deltaTime);

        if (Vector2.Distance(transform.position, Player1.position) < 1.5f)
        {
            Player1.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    void Flip()
    {
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void ResetFlip()
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
