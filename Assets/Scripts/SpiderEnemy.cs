using UnityEngine;

public class SpiderEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;
    public float detectionRange = 5f;
    public int damage = 1;
    public Transform player;
    public Animator animator;
    public Transform groundCheck;  // Assign empty GroundCheck object in Inspector
    public LayerMask groundLayer;

    private Vector3 initialPosition;
    private bool movingRight = false; // Now starts moving LEFT first
    private bool playerDetected = false;

    void Start()
    {
        initialPosition = transform.position;

        // Ensure the spider starts facing left (adjust if default sprite faces right)
        if (transform.localScale.x > 0)
        {
            FlipLeft();
        }
    }

    void Update()
    {
        if (playerDetected)
        {
            AttackPlayer();
        }
        else
        {
            if (IsGrounded())
            {
                Patrol();
                DetectPlayer();
            }
        }
    }

    void Patrol()
    {
        float patrolLimit = movingRight ? initialPosition.x + patrolDistance : initialPosition.x - patrolDistance;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(patrolLimit, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - patrolLimit) < 0.1f)
        {
            movingRight = !movingRight;
            FlipDirection();
        }
    }

    void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            playerDetected = true;
            animator.SetTrigger("Hostile");
        }
    }

    void AttackPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.position) < 1f)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void FlipDirection()
    {
        if (movingRight)
        {
            FlipRight(); // Flips only when moving right
        }
        else
        {
            FlipLeft(); // Stays flipped when moving left
        }
    }

    void FlipRight()
    {
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void FlipLeft()
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
