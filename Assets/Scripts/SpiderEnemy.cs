using UnityEngine;
using System.Collections;
using TMPro;

public class SpiderEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;
    public float detectionRange = 5f;
    public int damage = 1;
    public Transform player;
    public Animator animator;
    public Transform groundCheck;  // Assign empty GroundCheck object in Inspector
    public LayerMask groundLayer;
    public LightBluePlant bluePlant; // 🌱 Reference to LightBluePlant script

    public TMP_Text spiderDialogue; // 🗨️ Reference to TextMeshPro 3D
    private Vector3 initialPosition;
    private bool movingRight = false; // Now starts moving LEFT first
    private bool playerDetected = false;
    private bool plantIsDead = false; // 👀 Track if the plant is dead
    public GameObject magicShroomPrefab; // Assign this in the Inspector
    public Transform shroomThrowPoint; // Empty GameObject to set throw position
    public float throwForce = 3f; // Adjust the throw strength

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
        // Check if the plant is dead
        if (bluePlant != null && bluePlant.PlantDead && !plantIsDead)
        {
            plantIsDead = true; // Mark plant as dead
            ReactToPlantDeath(); // Stop being hostile and talk
        }

        if (plantIsDead) return; // Stop all enemy behavior if plant is dead

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
        if (plantIsDead) return; // 🛑 Stop detecting player if plant is gone

        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            playerDetected = true;
            animator.SetTrigger("Hostile");
        }
    }

    void AttackPlayer()
    {
        if (plantIsDead) return; // 🛑 Stop attacking if plant is gone

        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.position) < 1f)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
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

    // 🌱🐞 **React to Plant Death**
    void ReactToPlantDeath()
    {
        Debug.Log("The plant is dead! Spider stops being hostile.");
        playerDetected = false; // 🛑 Stop attacking
        animator.SetTrigger("Idle"); // 🕷️ Play idle animation

        StartCoroutine(FriendlySpiderSequence());
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
            "I guess I have no reason to be mad anymore.",
            "You seem nice after all!",
            "...",
            "I found this weird mushroom in the forest,",
            "You can have it as thanks.",
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

    IEnumerator FriendlySpiderSequence()
    {
        Debug.Log("Spider is now friendly!");

        // Show friendly dialogue
        StartCoroutine(SpiderTalkSequence());

        // Wait before saying goodbye
        yield return new WaitForSeconds(25f);

        // 🕷️ **Throw the Magic Mushroom!** 🍄
        if (magicShroomPrefab != null && shroomThrowPoint != null)
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
        Debug.Log("Spider is now passive and the player can pass!");

        Patrol();
    }
}
