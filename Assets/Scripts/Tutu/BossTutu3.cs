using TMPro;
using UnityEngine;
using System.Collections;

public class BossTutu3 : MonoBehaviour
{
    public Animator animator; // Reference to Tutu's animator
    public TMP_Text TutusText; // Reference to the TextMeshPro 3D object
    public TMP_Text UnstableText;
    public TMP_Text ItalicText;
    public float idleTime = 3f; // Time before Tutu starts talking

    public AudioSource tutuAudioSource;
    public AudioClip[] gibberishClips; // Array for different gibberish sounds (Optional)

    public GameObject poofEffectPrefab; // Assign PoofEffect prefab
    public Transform poofPosition; // Assign a specific spawn point if needed
    public AudioClip PoofCloudSound;

    private bool hasTalked = false; // Ensuring it only appears once per appearance

    void Start()
    {
        // Ensures Tutu starts in Invisible state
        animator.Play("Invisible");

        // Hide all text at the start
        if (TutusText) TutusText.gameObject.SetActive(false);
        if (UnstableText) UnstableText.gameObject.SetActive(false);
        if (ItalicText) ItalicText.gameObject.SetActive(false);

        // Starts the appear sequence
        StartCoroutine(AppearSequence());
    }

    IEnumerator AppearSequence()
    {
        // Wait a momement before appearing
        yield return new WaitForSeconds(2f);

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

        if (TutusText) TutusText.gameObject.SetActive(true);
        yield return StartCoroutine(ShowTextInSequence1());
        yield return StartCoroutine(ShowTextInSequence2A());

        if (UnstableText) UnstableText.gameObject.SetActive(true);
        yield return StartCoroutine(ShowTextInSequence2B());

        if (TutusText) yield return StartCoroutine(ShowTextInSequence3());
        yield return StartCoroutine(ShowTextInSequence4A());

        if (UnstableText) yield return StartCoroutine(ShowTextInSequence4B());
        yield return StartCoroutine(ShowTextInSequence5());

        if (ItalicText) ItalicText.gameObject.SetActive(true);
        yield return StartCoroutine(ShowTextInSequence6A());

        if (UnstableText) yield return StartCoroutine(ShowTextInSequence6B());

        // Hide all text
        if (TutusText) TutusText.gameObject.SetActive(false);
        if (UnstableText) UnstableText.gameObject.SetActive(false);
        if (ItalicText) ItalicText.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        // **Play poof effect again**
        GameObject poof = Instantiate(poofEffectPrefab, poofPosition.position, Quaternion.identity);
        Destroy(poof, 0.9f);

        // **Wait for poof animation, then disable Tutu**
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("Disappear");
    }

    IEnumerator ShowTextInSequence1()
    {
        yield return TypeText(TutusText, "Oh wow...\nYou really made it this far.");
        yield return TypeText(TutusText, "Apricot, you’re... impressive.");
    }

    IEnumerator ShowTextInSequence2A()
    {
        yield return TypeText(TutusText, "And here I thought you'd just—");
    }

    IEnumerator ShowTextInSequence2B()
    {
        yield return TypeText(UnstableText, "...fall into a pit and die by now.");
    }

    IEnumerator ShowTextInSequence3()
    {
        yield return TypeText(TutusText, "But no,\nYou kept pushing forward.");
        yield return TypeText(TutusText, "You fought, you won,\nYou actually LISTENED to me.");
    }

    IEnumerator ShowTextInSequence4A()
    {
        yield return TypeText(TutusText, "Do you have ANY IDEA...");
    }

    IEnumerator ShowTextInSequence4B()
    {
        yield return TypeText(UnstableText, "HOW FRUSTRATING THAT WAS??");
    }

    IEnumerator ShowTextInSequence5()
    {
        yield return TypeText(UnstableText, "I GUIDED YOU. I HELPED YOU. I LOVED YOU.");
        yield return TypeText(UnstableText, "AND YOU—");
        yield return TypeText(UnstableText, "YOU WERE NEVER SUPPOSED TO MAKE IT HERE!!");
    }

    IEnumerator ShowTextInSequence6A()
    {
        yield return TypeText(ItalicText, "Oh well!");
        yield return TypeText(ItalicText, "I guess I’ll just have to make sure you don’t make it any further.");
    }

    IEnumerator ShowTextInSequence6B()
    {
        yield return TypeText(UnstableText, "Let’s see how\nYou handle this,\nLittle kitten");
    }

    IEnumerator TypeText(TMP_Text textComponent, string fullText)
    {
        if (textComponent == null) yield break;

        textComponent.text = "";
        PlayRandomGibberish();

        foreach (char letter in fullText)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2f);
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
