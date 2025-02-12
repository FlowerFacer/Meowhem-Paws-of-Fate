using TMPro;
using UnityEngine;
using System.Collections;

public class BroomNPC : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float patrolDistance = 2.5f;
    public float detectionRange = 4f;
    public Transform player;
    public Animator animator;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Vector3 initialPosition;
    private bool movingRight = true;
    private bool playerDetected = false;
    private bool hasTalked = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (playerDetected && !hasTalked)
        {
            StartCoroutine(FriendlySpiderSequence());
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
        playerDetected = false;

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
        }
    }

    IEnumerator FriendlySpiderSequence()
    {
        moveSpeed = 0;
        Debug.Log("Spider is now friendly!");
        hasTalked = true; // ✅ Prevent repeat

        // Play "Bye" animation
        animator.SetTrigger("Bye");

        // Wait for animation to finish
        yield return new WaitForSeconds(3f);

        // **Start Passive Patrol**
        StartCoroutine(PassivePatrol());
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

    IEnumerator PassivePatrol()
    {
        while (true) // Runs forever
        {
            float patrolLimit = movingRight ? initialPosition.x + patrolDistance : initialPosition.x - patrolDistance;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(patrolLimit, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);

            if (Mathf.Abs(transform.position.x - patrolLimit) < 0.1f)
            {
                movingRight = !movingRight;
                UpdateFlip();
            }

            yield return null; // Wait for next frame
        }
    }
}
