using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 3.5f;
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] public float attackCooldown = 0.6f;

    private Collider2D col;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public Animator animator;
    public AnimatorOverrideController flippedAnimator; // Assign in Inspector
    private RuntimeAnimatorController defaultAnimator;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultAnimator = animator.runtimeAnimatorController; // Cache the default animator

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();  // Ensure sprite is correctly assigned
        col = GetComponent<Collider2D>();
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
        }
        else
        {
            animator.SetBool("isCrouching", false);
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


