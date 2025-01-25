using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 5f;
    [SerializeField]
    public float jumpForce = 10f;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();  // Ensure sprite is correctly assigned
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(3f, rb.linearVelocity.y);  // Constant right movement (for testing)

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump Pressed");
            animator.SetTrigger("isJumping");  // Force jump animation trigger for testing
        }

        HandleMovement();
        HandleJumping();
        HandleCrouching();
        HandleAttacking();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Update animation state
        animator.SetBool("isWalking", moveInput != 0);
        FlipSprite(moveInput);
    }

    void FlipSprite(float direction)
    {
        if (direction > 0)
            transform.localScale = new Vector3(0.23f, 0.23f, 1);
        else if (direction < 0)
            transform.localScale = new Vector3(-0.23f, 0.23f, 1);
    }

    void HandleJumping()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("isJumping");
        }
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.SetTrigger("isAttacking");
            StartCoroutine(ResetAttack());
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);  // Adjust based on animation length
        animator.ResetTrigger("isAttacking");
        animator.SetBool("isWalking", false);  // Return to idle or walking
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
}
