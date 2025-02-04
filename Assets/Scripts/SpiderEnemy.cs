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

        StartCoroutine(SpiderTalkSequence());
    }

    // 💬 **Spider Talks to Player**
    IEnumerator SpiderTalkSequence()
    {
        yield return new WaitForSeconds(1f);

        // 🗨️ Say dialogue one by one (Replace with UI system)
        Debug.Log("Spider: \"Oh... the plant is gone?\"");
        yield return new WaitForSeconds(2f);
        Debug.Log("Spider: \"I guess I have no reason to be mad anymore.\"");
        yield return new WaitForSeconds(2f);
        Debug.Log("Spider: \"You seem nice after all!\"");
        yield return new WaitForSeconds(2f);
        Debug.Log("Spider: \"Bye!\"");
        animator.SetTrigger("Bye");
    }

}
