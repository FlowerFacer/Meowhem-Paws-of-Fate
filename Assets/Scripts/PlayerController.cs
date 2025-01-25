using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed = 3.5f;
    [SerializeField]
    public float jumpForce = 10f;

    private Collider2D col;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public Animator animator;
    public AnimatorOverrideController flippedAnimator;  // Assign in Inspector
    private RuntimeAnimatorController defaultAnimator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        FlipSpecificSprite(false);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();  // Ensure sprite is correctly assigned
        col = GetComponent<Collider2D>();
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

        animator.SetBool("isWalking", moveInput != 0);

        FlipSprite(moveInput);
    }

    void FlipSprite(float direction)
    {
        Vector3 currentPosition = transform.position;
        float flipScale = 0.23f;  // Adjust as needed

        if (direction > 0 && transform.localScale.x < 0)
        {
            // Use the default animator (facing right)
            animator.runtimeAnimatorController = defaultAnimator;
            transform.localScale = new Vector3(flipScale, 0.23f, 1);
            transform.position = new Vector3(currentPosition.x + 0.9f, currentPosition.y, currentPosition.z);
        }
        else if (direction < 0 && transform.localScale.x > 0)
        {
            // Use the flipped animator (facing left)
            animator.runtimeAnimatorController = flippedAnimator;
            transform.localScale = new Vector3(-flipScale, 0.23f, 1);
            transform.position = new Vector3(currentPosition.x - 0.9f, currentPosition.y, currentPosition.z);
        }
    }

    void FlipSpecificSprite(bool facingLeft)
    {
        SpriteRenderer specificSprite = transform.Find("Knight Bow").GetComponent<SpriteRenderer>();

        if (facingLeft)
        {
            specificSprite.flipX = true;
        }
        else
        {
            specificSprite.flipX = false;

        }
    }

    void HandleJumping()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }
        else if (isGrounded)
        {
            animator.SetBool("isJumping", false);
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
