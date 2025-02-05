using UnityEngine;
using System.Collections;
using TMPro; // Import TextMeshPro

public class TutorialMouse : MonoBehaviour
{
    public Animator animator; // Reference to Tutu's animator
    public TMP_Text TutusText; // Reference to the TextMeshPro 3D object
    public float idleTime = 5f; // Time before Tutu starts talking
    public float talkDuration = 7f; // How long Tutu speaks before disappearing

    public AudioSource tutuAudioSource;
    public AudioClip[] gibberishClips; // Array for different gibberish sounds (Optional)

    public GameObject poofEffectPrefab; // Assign PoofEffect prefab
    public Transform poofPosition; // Assign a specific spawn point if needed

    private bool hasTalked = false; // Ensuring it only appears once per appearance

    void Start()
    {
        // Ensures Tutu starts in Invisible state
        animator.Play("Invisible");

        // Ensures Tutu's text is hidden at the start
        if (TutusText != null)
        {
            TutusText.gameObject.SetActive(false);
        }

        // Starts the appear sequence
        StartCoroutine(AppearSequence());
    }

    IEnumerator AppearSequence()
    {
        // Wait a momement before appearing
        yield return new WaitForSeconds(1f);

        // **Spawn Poof at Tutu's position**
        GameObject poof = Instantiate(poofEffectPrefab, poofPosition.position, Quaternion.identity);
        Destroy(poof, 0.9f);

        // Wait a bit for poof animation to finish
        yield return new WaitForSeconds(0.3f);

        // **Transition Tutu from Invisible to Idle**
        animator.SetTrigger("Appear");

        // Wait for the idle duration before speaking
        yield return new WaitForSeconds(idleTime);

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
        animator.SetTrigger("Disappear");
    }

    IEnumerator ShowTextInSequence()
    {
        string[] tutorialLines =
        {
            "Hi!",
            "My name is Tutu.",
            "I'm here to guide you!",
            "(So you don't die)",
            "*wink*",
            "Press A / D to move!",
            "Press Spacebar / W to jump!",
            "Press Control to crouch!",
            "...Good luck!"
        };

        foreach (string line in tutorialLines)
        {
            TutusText.text = "";
            PlayRandomGibberish(); // Plays gibberish at start of each line!

            foreach (char letter in line.ToCharArray())
            {
                TutusText.text += letter;
                yield return new WaitForSeconds(0.05f);
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

}

