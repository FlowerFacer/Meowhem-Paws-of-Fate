using TMPro;
using UnityEngine;
using System.Collections;

public class SpiderNPC : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float patrolDistance = 3f;
    public float detectionRange = 5f;
    public Transform player;
    public Animator animator;
    public Transform groundCheck;  // Assign empty GroundCheck object in Inspector
    public LayerMask groundLayer;

    public TMP_Text spiderDialogue; // 🗨️ Reference to TextMeshPro 3D
    private Vector3 initialPosition;
    private bool movingRight = false; // Now starts moving LEFT first
    private bool playerDetected = false;

    private bool hasTalked = false; // ✅ Prevents repeating FriendlySpiderSequence


    void Start()
    {
        initialPosition = transform.position;

        // Change Spider's layer to NonBlockingNPC so the player can walk through
        gameObject.layer = LayerMask.NameToLayer("NonBlockingNPC");

        // 🎉 Spider stays but is now passive
        Debug.Log("Spider is now passive and the player can pass!");

        // Ensure the spider starts facing left (adjust if default sprite faces right)
        if (transform.localScale.x > 0)
        {
            FlipLeft();
        }
    }

    void Update()
    {
        if (playerDetected && !hasTalked)
        {
            StartCoroutine(FriendlySpiderSequence());
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

        if (Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            playerDetected = true;
            StartCoroutine(SpiderTalkSequence());
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

    // 💬 **Spider Talks to Player**
    IEnumerator SpiderTalkSequence()
    {
        animator.SetBool("Idle", true);

        // **Stop other running text coroutines**
        StopAllCoroutines();

        yield return new WaitForSeconds(1f);

        if (spiderDialogue != null)
        {
            spiderDialogue.gameObject.SetActive(true);
            spiderDialogue.text = "Bye!"; // ✅ Ensure text is set correctly

            // ✅ **Parent to spider for smooth movement**
            spiderDialogue.transform.SetParent(transform);

            // ✅ **Set position once to avoid jittering**
            spiderDialogue.transform.localPosition = new Vector3(0, 1.5f, 0);

            // ✅ **Fade in effect**
            StartCoroutine(FadeText(spiderDialogue, true));
        }

        yield return new WaitForSeconds(2f);

        // Hide text
        if (spiderDialogue != null)
        {
            spiderDialogue.gameObject.SetActive(false);
            spiderDialogue.transform.SetParent(null); // Remove parent after hiding
        }
    }

    IEnumerator FriendlySpiderSequence()
    {
        moveSpeed = 0;
        Debug.Log("Spider is now friendly!");
        hasTalked = true; // ✅ Prevent repeat

        // Show friendly dialogue
        StartCoroutine(SpiderTalkSequence());

        // Play "Bye" animation
        animator.SetTrigger("Bye");

        // Wait for animation to finish
        yield return new WaitForSeconds(3f);

        // **Start Passive Patrol**
        StartCoroutine(PassivePatrol());
    }

    IEnumerator PassivePatrol()
    {
        while (true) // Runs forever
        {
            moveSpeed = 1;

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

    IEnumerator FadeText(TMP_Text textObj, bool fadeIn)
    {
        float duration = 0.5f;
        float elapsedTime = 0f;
        Color color = textObj.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = fadeIn ? Mathf.Lerp(0, 1, elapsedTime / duration) : Mathf.Lerp(1, 0, elapsedTime / duration);
            textObj.color = color;
            yield return null;
        }

        if (!fadeIn)
        {
            textObj.gameObject.SetActive(false);
        }
    }

}
