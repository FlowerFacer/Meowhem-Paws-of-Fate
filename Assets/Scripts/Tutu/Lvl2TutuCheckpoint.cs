using TMPro;
using UnityEngine;
using System.Collections;

public class Lvl2TutuCheckpoint : MonoBehaviour
{
    public Animator animator; // Reference to Tutu's animator
    public TMP_Text TutusText; // Reference to the TextMeshPro 3D object
    public float idleTime = 1f; // Time before Tutu starts talking
    public float talkDuration = 12f; // How long Tutu speaks before disappearing

    public AudioSource tutuAudioSource;
    public AudioClip[] gibberishClips; // Array for different gibberish sounds (Optional)
    public AudioClip Gibbersh;
    public AudioClip PoofCloudSound;

    public GameObject poofEffectPrefab; // Assign PoofEffect prefab
    public Transform poofPosition; // Assign a specific spawn point if needed

    private bool hasTalked = false; // Ensuring it only appears once per appearance
    private bool isTalking = false; // Track if Tutu is talking
    private bool hasSaidStop = false;
    public GameObject tutuGameObject; // The entire Tutu prefab (Disable at start)
    public bool playerSkipped = false; // detect player skipping

    private Coroutine talkCoroutine; // Store running talk coroutine


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
        if (Input.GetKeyDown(KeyCode.Return) && isTalking)
        {
            StopTutuDialogue(); // Immediately stop and trigger "RUDE!"
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTalked && !hasSaidStop) // Player reaches checkpoint
        {
            StartCoroutine(AppearSequence());
        }
    }

    IEnumerator AppearSequence()
    {
        hasSaidStop = true;

        if (PoofCloudSound != null)
        {
            AudioManager.instance.PlaySound(PoofCloudSound);
        }

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
        isTalking = true;

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

        isTalking = false;

        // Hide tutorial text
        if (TutusText != null)
        {
            TutusText.gameObject.SetActive(false);
        }

        if (PoofCloudSound != null)
        {
            AudioManager.instance.PlaySound(PoofCloudSound);
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
            "This is very important,\n",
            "So pay attention:",
            "I love you.",
            "...",
            "Okay, bye!"
        };

        foreach (string line in tutorialLines2)
        {
            TutusText.text = "";
            PlayRandomGibberish(); // Plays gibberish at start of each line!

            foreach (char letter in line.ToCharArray())
            {
                TutusText.text += letter;
                yield return new WaitForSeconds(0.05f);

                if (playerSkipped) yield break; // Stop if player skips
            }

            yield return new WaitForSeconds(1.5f);
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

    void StopTutuDialogue()
    {
        playerSkipped = true;
        isTalking = false;

        animator.SetTrigger("Rude!");

        if (talkCoroutine != null)
        {
            StopCoroutine(talkCoroutine); // Stop the speaking coroutine
        }

        StartCoroutine(HandlePlayerSkip());
    }

    IEnumerator HandlePlayerSkip()
    {
        string text = "RUDE!";
        TutusText.text = "";

        if (Gibbersh != null)
        {
            AudioManager.instance.PlaySound(Gibbersh);
        }

        foreach (char letter in text)
        {
            TutusText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1.8f);

        StartCoroutine(HideTutu());
    }

    IEnumerator HideTutu()
    {
        if (TutusText != null)
        {
            TutusText.gameObject.SetActive(false);
        }

        if (PoofCloudSound != null)
        {
            AudioManager.instance.PlaySound(PoofCloudSound);
        }

        GameObject poof = Instantiate(poofEffectPrefab, poofPosition.position, Quaternion.identity);
        Destroy(poof, 0.9f);

        yield return new WaitForSeconds(0.1f); // ✅ Coroutine requires yield

        if (tutuGameObject != null)
        {
            tutuGameObject.SetActive(false);
        }
    }
}
