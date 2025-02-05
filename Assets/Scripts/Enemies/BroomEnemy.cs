using UnityEngine;

public class BroomEnemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float patrolDistance = 2.5f;
    public float detectionRange = 4f;
    public int damage = 2;
    public Transform player;
    public Animator animator;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Vector3 initialPosition;
    private bool movingRight = true;
    private bool playerDetected = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Keep the broom at a fixed Y position
        transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
    }

    void FixedUpdate()
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
            UpdateFlip();
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
        // Move only in the X-axis (prevents jittering)
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * 1.5f * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.position) < 1.5f)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

        // Flip the broom based on direction
        if (player.position.x > transform.position.x)
            FlipRight();
        else
            FlipLeft();
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void UpdateFlip()
    {
        if (movingRight)
        {
            FlipRight();
        }
        else
        {
            FlipLeft();
        }
    }

    void FlipRight()
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void FlipLeft()
    {
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
