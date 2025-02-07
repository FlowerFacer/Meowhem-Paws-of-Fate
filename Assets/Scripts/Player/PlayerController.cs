using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Security.Cryptography;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 3.5f;
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] public float attackCooldown = 0.6f;
    [SerializeField] public float specialAttackCooldown = 2.4f;
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

    public GameObject magicTransformPrefab; // Ref to Magic Transformation prefab
    public GameObject idleTransformPrefab; // Ref to Idle Transformation prefab
    public GameObject slashEffectPrefab; // Ref to slash prefab
    public float attackDuration = 0.3f;   // Time before slash disappears
    public Transform attackPoint;         // Empty object where slash appears
    public GameObject LightningEffectPrefab; // Ref to lightning prefab
    public float LightningDuration = 0.8f; // Time before lightning disappears
    public Transform LightningPoint; // Empty object where lightning appears

    public AudioClip LightningSound;
    public AudioClip SlashSound;
    public AudioClip fireEffectSound;

    private SpriteRenderer spriteRenderer;
    public ParticleSystem SmokeFX;
    private Rigidbody2D rb;

    private bool isGrounded;
    private bool isAttacking = false;
    public bool isSpecialAttacking = false;
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.4f, groundLayer);
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
                // Check if player is moving
        bool isMoving = moveInput != 0;

        // Apply movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (isGrounded)
        {
            // Enable walking animation if moving
            animator.SetBool("isWalking", isMoving);

            // Handle dust effect when moving
            if (isMoving)
            {
                if (!SmokeFX.isPlaying)
                {
                    SmokeFX.Play();
                }
            }
            else
            {
                SmokeFX.Stop(); // Stop dust effect when not moving
            }
        }
        else
        {
            // Player is in the air -> Disable walking animation & stop dust effect
            animator.SetBool("isWalking", false);
            SmokeFX.Stop();
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

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
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
        if (Input.GetMouseButtonDown(1) && !isSpecialAttacking)
        {
            StartCoroutine(PerformSpecialAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;

        if (SlashSound != null)
        {
            AudioManager.instance.PlaySound(SlashSound);
        }

        // Trigger attack animation
        animator.SetTrigger("isAttack");
        GameObject slash = Instantiate(slashEffectPrefab, attackPoint.position, attackPoint.rotation);
        Destroy(slash, attackDuration); // Remove after duration

        // Wait for attack animation to finish
        yield return new WaitForSeconds(attackCooldown);

        // Reset attacking state
        isAttacking = false;
    }

    IEnumerator PerformSpecialAttack()
    {
        if (fireEffectSound != null)
        {
            AudioManager.instance.PlaySound(fireEffectSound);
        }

        // Spawn blue fire slightly higher to match the enemy's body
        if (magicTransformPrefab != null)
        {
            Vector3 explosionPosition = transform.position + new Vector3(-0.5f, 1f, 0); // Adjust Y position
            GameObject BlueFire = Instantiate(magicTransformPrefab, explosionPosition, Quaternion.identity);
            Destroy(BlueFire, 0.8f); // Destroy after animation finishes
        }

        //yield return new WaitForSeconds(0.2f); // Wait for animation to finish

        isSpecialAttacking = true;

        animator.SetTrigger("isSpecialAttack");

        yield return new WaitForSeconds(0.5f); // Wait for animation to finish

        if (LightningSound != null)
        {
            AudioManager.instance.PlaySound(LightningSound);
        }

        yield return new WaitForSeconds(1.5f); // Wait for animation to progress
        GameObject Lightning = Instantiate(LightningEffectPrefab, LightningPoint.position, LightningPoint.rotation);
        Destroy(Lightning, LightningDuration); // Remove after duration

        // Wait for attack animation to finish
        // yield return new WaitForSeconds(specialAttackCooldown);

        yield return new WaitForSeconds(1.9f); // Wait for animation to finish

        // Spawn purple fire slightly higher to match the enemy's body
        if (idleTransformPrefab != null)
        {
            Vector3 explosionPosition = transform.position + new Vector3(-0.5f, 1f, 0); // Adjust Y position
            GameObject PurpleFire = Instantiate(idleTransformPrefab, explosionPosition, Quaternion.identity);
            Destroy(PurpleFire, 0.8f); // Destroy after animation finishes
        }

        // Reset attacking state
        isSpecialAttacking = false;
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


