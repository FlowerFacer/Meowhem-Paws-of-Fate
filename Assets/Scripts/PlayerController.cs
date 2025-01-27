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

    bool wasGrounded = true; //tracks previous grounded state

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
        // Ensure we cache the default animator at the start of the game
        defaultAnimator = animator.runtimeAnimatorController;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();  // Ensure sprite is correctly assigned
        col = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(3f, rb.linearVelocity.y);  // Constant right movement (for testing)

        HandleMovement();
        HandleCrouching();
    }

    void Update()
    {
        HandleJumping();
        HandleMovement();  // Only allow movement when grounded
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (isGrounded)
        {
            animator.SetBool("isWalking", moveInput != 0);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        FlipSprite(moveInput);
    }

    void FlipSprite(float direction)
    {
        Vector3 currentPosition = transform.position;
        float flipScale = 0.23f;  // Adjust as needed

        if (direction > 0 && transform.localScale.x < 0 && animator.runtimeAnimatorController != defaultAnimator)
        {
            // Use the default animator (facing right)
            animator.runtimeAnimatorController = defaultAnimator;
            transform.localScale = new Vector3(flipScale, 0.23f, 1);
            transform.position = new Vector3(currentPosition.x + 0.9f, currentPosition.y, currentPosition.z);
        }
        else if (direction < 0 && transform.localScale.x > 0 && animator.runtimeAnimatorController != flippedAnimator)
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

        if (isGrounded && !wasGrounded)  // Set Landed only when transitioning from air to ground
        {
            animator.SetTrigger("Landed");
            animator.SetBool("isJumping", false);
            wasGrounded = true;
        }

        if (!isGrounded && wasGrounded)
        {
            animator.SetBool("isJumping", true);
            wasGrounded = false;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            PerformJump();
        }
    }

    void PerformJump()
    {
        animator.ResetTrigger("Landed");  // Reset Landed trigger to avoid interference
        animator.SetTrigger("JumpTrigger");  // Trigger jump animation
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        //StartCoroutine(EndJumpAnimation());
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

    IEnumerator EndJumpAnimation()
    {
        yield return new WaitForSeconds(0.6f);  // Wait for animation duration
        animator.SetTrigger("Landed");  // Transition back to idle
    }

    IEnumerator ResetJumpTrigger()
    {
        yield return new WaitForSeconds(0.6f);
        animator.ResetTrigger("JumpTrigger");
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
