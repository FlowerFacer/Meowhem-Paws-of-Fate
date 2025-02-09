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

        // **Ensure all text starts hidden**
        HideAllText();

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

        // **Main text sequence**
        yield return ShowText(TutusText, "Oh wow...\n You really made\n it this far.", 2f);
        yield return ShowText(TutusText, "Apricot, you’re...\n impressive.", 2f);

        yield return ShowText(TutusText, "And here I thought\n you'd just—", 3f);
        yield return ShowText(UnstableText, "...fall into a pit\n and die by now.", 1.5f);

        yield return ShowText(TutusText, "But no, you kept\n pushing forward.", 2f);
        yield return ShowText(TutusText, "You fought, you won,\n you actually\n LISTENED to me.", 2f);

        yield return ShowText(TutusText, "Do you have\n ANY IDEA...", 1.5f);
        yield return ShowText(UnstableText, "HOW\n FRUSTRATING\n THAT WAS??", 2f);

        yield return ShowText(UnstableText, "I GUIDED YOU.\n I HELPED YOU.\n I PRETENDED\n TP LOVE YOU.", 2.5f);
        yield return ShowText(UnstableText, "AND YOU—", 1.5f);
        yield return ShowText(UnstableText, "YOU WERE NEVER\n SUPPOSED TO\n MAKE IT HERE!!", 2.5f);

        yield return ShowText(ItalicText, "Oh well!", 1.5f);
        yield return ShowText(ItalicText, "I guess I’ll just\n have to make sure...", 2f);
        yield return ShowText(ItalicText, "You don’t make it\n any further.", 2f);

        yield return ShowText(UnstableText, "Let’s see how\n you handle this,\n little kitten...", 2f);

        // **Disappear effect**
        HideAllText();

        yield return new WaitForSeconds(200f);

        // **Play poof effect again**
        GameObject poof = Instantiate(poofEffectPrefab, poofPosition.position, Quaternion.identity);
        Destroy(poof, 0.9f);

        // **Wait for poof animation, then disable Tutu**
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("Disappear");
    }

    IEnumerator ShowText(TMP_Text textObject, string text, float delay)
    {
        HideAllText(); // **Ensure only one text is shown at a time**
        textObject.gameObject.SetActive(true);
        textObject.text = "";

        PlayRandomGibberish();

        foreach (char letter in text)
        {
            textObject.text += letter;
            yield return new WaitForSeconds(0.065f);
        }

        yield return new WaitForSeconds(delay);
    }

    void HideAllText()
    {
        TutusText.gameObject.SetActive(false);
        UnstableText.gameObject.SetActive(false);
        ItalicText.gameObject.SetActive(false);
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
