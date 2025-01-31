using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Security.Cryptography;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 3.5f;
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] public float attackCooldown = 0.6f;
    [SerializeField] public float climbJumpForceX = 2f; // Small jump to the right
    [SerializeField] public float climbJumpForceY = 2f; // Slight upward force

    private Collider2D col;
    private BoxCollider2D playerCollider;
    private Vector2 originalSize;
    private Vector2 originalOffset;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public Animator animator;
    public AnimatorOverrideController flippedAnimator; // Assign in Inspector
    private RuntimeAnimatorController defaultAnimator;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isAttacking = false;
    private bool isClimbing = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultAnimator = animator.runtimeAnimatorController; // Cache the default animator

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();  // Ensure sprite is correctly assigned
        col = GetComponent<Collider2D>();

        playerCollider = GetComponent<BoxCollider2D>();  // Get the player's collider
        originalSize = playerCollider.size;  // Store original collider size
        originalOffset = playerCollider.offset;  // Store original collider offset
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void Update()
    {
        HandleMovement();
        HandleCrouching();
        HandleMovement();
        HandleJumping();
        HandleAttacking();

        if (isClimbing)
        {
            HandleClimbing(); // Allow player control while climbing
        }
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Apply movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (isGrounded)
        {
            animator.SetBool("isWalking", moveInput != 0);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // Flip player sprite
        if (moveInput > 0)
        {
            FlipSprite(1); // Face right
        }
        else if (moveInput < 0)
        {
            FlipSprite(-1); // Face left
        }
    }

    void HandleJumping()
    {
        if (isClimbing) return; // Prevent jumping while climbing

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("JumpTrigger");
        }

        animator.SetBool("isJumping", !isGrounded);
    }

    void HandleCrouching()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            animator.SetBool("isCrouching", true);

            // Reduce collider size and adjust offset
            playerCollider.size = new Vector2(originalSize.x, originalSize.y / 2);
            playerCollider.offset = new Vector2(originalOffset.x, originalOffset.y - (originalSize.y / 4));
        }
        else
        {
            animator.SetBool("isCrouching", false);

            // Restore original collider size and offset
            playerCollider.size = originalSize;
            playerCollider.offset = originalOffset;
        }
    }

    void HandleClimbing()
    {
        float climbInput = Input.GetAxisRaw("Vertical"); // W = 1, S = -1

        if (climbInput != 0)
        {
            rb.linearVelocity = new Vector2(0, climbInput * 3f); // Move up/down
            animator.SetBool("isClimbing", true);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, 0); // Stop moving when no input
            animator.SetBool("isClimbing", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            if (transform.localScale.x < 0) // Facing left? Prevent climbing!
            {
                Debug.Log("Can't climb while facing left!");
                return;
            }

            isClimbing = true;
            rb.gravityScale = 0; // Disable gravity for smooth climbing
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isClimbing", true);
        }
        else if (other.CompareTag("LeftLadder"))
        {
            if (transform.localScale.x > 0) // Facing left? Prevent climbing!
            {
                Debug.Log("Can't climb while facing right!");
                return;
            }

            isClimbing = true;
            rb.gravityScale = 0; // Disable gravity for smooth climbing
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isClimbing", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 2.5f; // Restore gravity
            animator.SetBool("isClimbing", false);
        }
        else if (other.CompareTag("LeftLadder"))
        {
            isClimbing = false;
            rb.gravityScale = 2.5f; // Restore gravity
            animator.SetBool("isClimbing", false);
        }
    }

    void HandleAttacking()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }

        // Placeholder for right mouse click special attack
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Special attack coming soon!");
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;

        // Trigger attack animation
        animator.SetTrigger("isAttack");

        // Wait for attack animation to finish
        yield return new WaitForSeconds(attackCooldown);

        // Reset attacking state
        isAttacking = false;
    }

    void FlipSprite(float direction)
    {
        Vector3 currentPosition = transform.position;
        float flipScale = 0.23f; // Adjust as needed

        if (direction > 0 && transform.localScale.x < 0)
        {
            // Flip to face right
            transform.localScale = new Vector3(flipScale, transform.localScale.y, 1);
            animator.runtimeAnimatorController = defaultAnimator;
            AdjustPosition(0.9f); // Smooth position adjustment for right-facing sprite
        }
        else if (direction < 0 && transform.localScale.x > 0)
        {
            // Flip to face left
            transform.localScale = new Vector3(-flipScale, transform.localScale.y, 1);
            animator.runtimeAnimatorController = flippedAnimator;
            AdjustPosition(-0.9f); // Smooth position adjustment for left-facing sprite
        }
    }

    void AdjustPosition(float offset)
    {
        // Prevents teleportation by slightly adjusting the position during flipping
        transform.position = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
}


