using TMPro;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    public TMP_Text messageText; // Assign a TextMeshPro UI Text in Inspector
    public string message = "Default Message"; // Custom message for this trigger

    //

    private void Start()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false); // Hide text at start
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true); // Show text when player enters
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && messageText != null)
        {
            messageText.gameObject.SetActive(false); // Hide text when player exits
        }
    }
}
