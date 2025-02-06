using UnityEngine;
using System.Collections;
using TMPro; // Import TextMeshPro
using UnityEngine.InputSystem;


public class Part2TutorialMouse : MonoBehaviour
{
    public Animator animator; // Reference to Tutu's animator
    public TMP_Text TutusText; // Reference to the TextMeshPro 3D object
    public float idleTime = 1f; // Time before Tutu starts talking
    public float talkDuration = 30f; // How long Tutu speaks before disappearing

    public AudioSource tutuAudioSource;
    public AudioClip[] gibberishClips; // Array for different gibberish sounds (Optional)
    public AudioClip Gibbersh;

    public GameObject poofEffectPrefab; // Assign PoofEffect prefab
    public Transform poofPosition; // Assign a specific spawn point if needed

    private bool hasTalked = false; // Ensuring it only appears once per appearance
    public GameObject tutuGameObject; // The entire Tutu prefab (Disable at start)
    public bool playerSkipped = false; // detect player skipping


    void Start()
    {
        // Ensures Tutu starts in Invisible state
        animator.Play("Invisible");

        // Ensures Tutu's text is hidden at the start
        if (TutusText != null)
        {
            TutusText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerSkipped)
        {
            HandlePlayerSkip();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTalked) // Player reaches checkpoint
        {
            StartCoroutine(AppearSequence());
        }
    }

    IEnumerator AppearSequence()
    {
        // **Spawn Poof at Tutu's position**
        GameObject poof = Instantiate(poofEffectPrefab, poofPosition.position, Quaternion.identity);
        Destroy(poof, 0.9f);

        // Wait a bit for poof animation to finish
        yield return new WaitForSeconds(0.3f);

        // **Transition Tutu from Invisible to Idle**
        animator.SetTrigger("Appear");

        // Wait for the idle duration before speaking
        yield return new WaitForSeconds(idleTime);

        animator.SetTrigger("Stop!");

        // Show tutorial text
        if (TutusText != null)
        {
            TutusText.gameObject.SetActive(true);
            StartCoroutine(ShowTextStop()); // Display text with typewriter effect
        }

        // Wait for the talk duration before disappearing
        yield return new WaitForSeconds(3f);

        if (!hasTalked)
        {
            StartCoroutine(TalkSequence());
        }
    }

    IEnumerator TalkSequence()
    {
        hasTalked = true; // Ensure it doesn't repeat

        // Play talking animation
        animator.SetBool("IsTalking", true);

        // Show tutorial text
        if (TutusText != null)
        {
            TutusText.gameObject.SetActive(true);
            StartCoroutine(ShowTextInSequence()); // Display text with typewriter effect
        }

        // Wait for the talk duration before disappearing
        yield return new WaitForSeconds(talkDuration);

        // Hide tutorial text
        if (TutusText != null)
        {
            TutusText.gameObject.SetActive(false);
        }

        // **Play poof effect again**
        GameObject poof = Instantiate(poofEffectPrefab, poofPosition.position, Quaternion.identity);
        Destroy(poof, 0.9f);

        // **Wait for poof animation, then disable Tutu**
        yield return new WaitForSeconds(0.1f);
        if (tutuGameObject != null)
        {
            tutuGameObject.SetActive(false);
        }
    }

    IEnumerator ShowTextStop()
    {
        string text = "STOP!";
        TutusText.text = "";

        if (Gibbersh != null)
        {
            AudioManager.instance.PlaySound(Gibbersh);
        }

        foreach (char letter in text) { TutusText.text += letter; yield return new WaitForSeconds(0.05f); }
    }

    IEnumerator ShowTextInSequence()
    {
        string[] tutorialLines2 =
{
            "If you go down this ladder,",
            "You will encounter Vivi!",
            "She is usually very friendly,",
            "But someone put a light blue\n plant over there -",
            "And she hates it.",
            "So now, prepare to fight!!",
            "Press the Left Mouse Button\n to attack!",
            "Press the Right Mouse Button\n to perform Special Attack!",
            "Use Special Attack wisely,",
            "There is a 60 second\n cooldown period.",
            "DON'T DIE!"
        };

        foreach (string line in tutorialLines2)
        {
            TutusText.text = "";
            PlayRandomGibberish(); // Plays gibberish at start of each line!

            foreach (char letter in line.ToCharArray())
            {
                TutusText.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(1.5f);

            if (Input.GetKey(KeyCode.Return))
            {
                playerSkipped = true;
            }
        }
    }

    void PlayRandomGibberish()
    {
        if (tutuAudioSource != null)
        {
            tutuAudioSource.pitch = Random.Range(0.9f, 1.2f); // Random pitch variation
            tutuAudioSource.clip = gibberishClips[Random.Range(0, gibberishClips.Length)]; // Pick a random clip
            tutuAudioSource.Play();
        }
    }

    IEnumerator HandlePlayerSkip()
    {
        string text = "STOP!";
        TutusText.text = "";

        if (Gibbersh != null)
        {
            AudioManager.instance.PlaySound(Gibbersh);
        }

        foreach (char letter in text) { TutusText.text += letter; yield return new WaitForSeconds(0.05f); }

        yield return new WaitForSeconds(1f);

        // Hide tutorial text
        if (TutusText != null)
        {
            TutusText.gameObject.SetActive(false);
        }

        // **Play poof effect again**
        GameObject poof = Instantiate(poofEffectPrefab, poofPosition.position, Quaternion.identity);
        Destroy(poof, 0.9f);

        // **Wait for poof animation, then disable Tutu**
        yield return new WaitForSeconds(0.1f);
        if (tutuGameObject != null)
        {
            tutuGameObject.SetActive(false);
        }
    }
}

