using UnityEngine;
using System.Collections;
using TMPro;

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
    public float attackCooldown = 2f; // Time between attacks
    public bool SockIsDead = false;
    public PlayerHealth playerHealth;
    public PurpleSock purpleSock;
    public GameObject purpleSockPrefab; // Assign this in the Inspector
    public bool playerIsDead = false;

    public GameObject magicShroomPrefab; // Assign this in the Inspector
    public Transform shroomThrowPoint; // Empty GameObject to set throw position
    public float throwForce = 6f; // Adjust the throw strength

    public TMP_Text AngryRant; // Repeating text
    public TMP_Text broomDialogue; // 🗨️ Reference to TextMeshPro 3D

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Check if the sock is dead
        if (purpleSock != null && purpleSock.SockDead && !SockIsDead)
        {
            SockIsDead = true;
            Debug.Log("🔴 Spider detected that sock is DEAD.");
            ReactToSockDeath();
        }

        // If the player is dead, stop movement and attack
        if (!playerIsDead && playerHealth.isDead)
        {
            playerIsDead = true; // Mark player as dead
            ReactToPlayerDeath();
        }

        if (SockIsDead || playerIsDead) return; // Stop broom logic if necessary

        // Keep the broom at a fixed Y position
        transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);

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
        if (SockIsDead) return; // 🛑 Stop detecting player if sock is gone

        if (playerIsDead) return;

        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            playerDetected = true;
            animator.SetTrigger("SweepAttack");
        }
    }

    void AttackPlayer()
    {
        if (SockIsDead) return; // 🛑 Stop attacking if sock is gone

        if (playerIsDead) return;

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

    // 🌱🐞 **React to sock Death**
    void ReactToSockDeath()
    {
        Debug.Log("The sock is dead! Broom stops being hostile.");
        playerDetected = false; // 🛑 Stop attacking
        animator.SetTrigger("Idle"); // Play idle animation

        StartCoroutine(FriendlyBroomSequence());
    }

    void ReactToPlayerDeath()
    {
        Debug.Log("The player is dead! Broom stops being hostile.");
        playerDetected = false; // 🛑 Stop attacking
        animator.SetTrigger("Idle"); // 🕷️ Play idle animation

        StartCoroutine(PassivePatrol());
    }

    // 💬 **broom Talks to Player**
    IEnumerator BroomTalkSequence()
    {
        yield return new WaitForSeconds(1f);

        // 🗨️ Enable and position dialogue text
        if (broomDialogue != null)
        {
            broomDialogue.gameObject.SetActive(true);
            broomDialogue.transform.position = transform.position + new Vector3(0, 1.5f, 0); // Adjust height
        }

        string[] ViviLines =
{
            "Huh.",
            "You destroyed\n the stinky sock?",
            "Some lunatic placed\n it there to prank me.",
            "...!",
            "Thanks dude.",
            "This is yours,",
            "Make good use of it.",
            "Goodbye!"
        };

        foreach (string line in ViviLines)
        {
            broomDialogue.text = "";

            foreach (char letter in line.ToCharArray())
            {
                broomDialogue.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(1.5f);
        }

        // Hide text
        if (broomDialogue != null)
        {
            broomDialogue.gameObject.SetActive(false);
        }
    }

    IEnumerator FriendlyBroomSequence()
    {
        Debug.Log("Broom is now friendly!");

        // Show friendly dialogue
        StartCoroutine(BroomTalkSequence());

        // Wait before saying goodbye
        yield return new WaitForSeconds(20f);

        // 🕷️ **Throw the Magic Mushroom!** 🍄
        if (purpleSockPrefab != null && shroomThrowPoint != null)
        {
            GameObject thrownShroom = Instantiate(magicShroomPrefab, shroomThrowPoint.position, Quaternion.identity);
            Rigidbody2D shroomRb = thrownShroom.GetComponent<Rigidbody2D>();

            if (shroomRb != null)
            {
                // Apply force to throw the mushroom forward
                shroomRb.linearVelocity = new Vector2(transform.localScale.x * throwForce, 2f); // Adjust arc if needed
            }
        }

        yield return new WaitForSeconds(1f);

        // Play "Bye" animation
        animator.SetTrigger("Bye");

        // Wait for animation to finish
        yield return new WaitForSeconds(1f);

        // Change Spider's layer to NonBlockingNPC so the player can walk through
        gameObject.layer = LayerMask.NameToLayer("NonBlockingNPC");

        // 🎉 Spider stays but is now passive
        Debug.Log("Broom is now passive and the player can pass!");

        // **Start Passive Patrol**
        StartCoroutine(PassivePatrol());
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
