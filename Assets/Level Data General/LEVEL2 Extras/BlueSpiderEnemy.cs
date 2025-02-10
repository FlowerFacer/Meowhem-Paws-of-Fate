using TMPro;
using UnityEngine;
using System.Collections;

public class BlueSpiderEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;
    public float detectionRange = 5f;
    public int damage = 1;
    public Transform player;
    public Animator animator;
    public Transform groundCheck;  // Assign empty GroundCheck object in Inspector
    public LayerMask groundLayer;

    public TMP_Text spiderDialogue; // 🗨️ Reference to TextMeshPro 3D
    private Vector3 initialPosition;
    private bool movingRight = false; // Now starts moving LEFT first
    private bool playerDetected = false;

    public GameObject spiderAttackPrefab; // The prefab of the attack effect (web)
    public Transform attackSpawnPoint; // Empty GameObject where the attack spawns
    public float attackCooldown = 2f; // Time between attacks
    private bool canAttack = true; // Prevent spamming attacks
    public bool playerIsDead;
    public PlayerHealth playerHealth;

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

        // If the player is dead, stop movement and attack
        if (!playerIsDead && playerHealth.isDead)
        {
            playerIsDead = true; // Mark player as dead
            ReactToPlayerDeath();
        }

        if (playerIsDead) return; // Stop spider logic if necessary


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
        if (playerIsDead) return;

        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            playerDetected = true;
            animator.SetTrigger("Hostile");
        }
    }

    void AttackPlayer()
    {
        if (playerIsDead) return;

        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.position) < 1f && canAttack)
        {
            StartCoroutine(PerformAttack());
        }
    }

    IEnumerator PerformAttack()
    {
        canAttack = false; // Disable attacks temporarily

        // **Spawn the Spider Web Attack Effect**
        if (spiderAttackPrefab != null && attackSpawnPoint != null)
        {
            Instantiate(spiderAttackPrefab, attackSpawnPoint.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(attackCooldown); // Wait before attacking again

        canAttack = true; // Enable attacks again
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

    void ReactToPlayerDeath()
    {
        Debug.Log("The player is dead! Spider stops being hostile.");
        playerDetected = false; // 🛑 Stop attacking
        animator.SetTrigger("Idle"); // 🕷️ Play idle animation

        StartCoroutine(PassivePatrol());
    }

    // 💬 **Spider Talks to Player**
    IEnumerator SpiderTalkSequence()
    {
        yield return new WaitForSeconds(1f);

        // 🗨️ Enable and position dialogue text
        if (spiderDialogue != null)
        {
            spiderDialogue.gameObject.SetActive(true);
            spiderDialogue.transform.position = transform.position + new Vector3(0, 1.5f, 0); // Adjust height
        }

        string[] ViviLines =
{
            "Oh...",
            "The plant is gone?",
            "I guess I don't have a reason\n to be mad anymore.",
            "You seem nice after all!",
            "...",
            "I found this weird mushroom\n in the forest,",
            "So you can have it as thanks.",
            "Bye!"
        };

        foreach (string line in ViviLines)
        {
            spiderDialogue.text = "";

            foreach (char letter in line.ToCharArray())
            {
                spiderDialogue.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(1.5f);
        }

        // Hide text
        if (spiderDialogue != null)
        {
            spiderDialogue.gameObject.SetActive(false);
        }
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
                FlipDirection();
            }

            yield return null; // Wait for next frame
        }
    }
}
