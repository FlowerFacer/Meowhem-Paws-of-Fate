using TMPro;
using UnityEngine;

public class TutuCheckpointDisabler : MonoBehaviour
{
    public GameObject tutu1; // Assign Tutu1 GameObject in the Inspector
    public TMP_Text TutusText; // Reference to the TextMeshPro 3D object

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && tutu1 != null) // If the player reaches the checkpoint
        {
            tutu1.SetActive(false); // Disable Tutu1 to stop talking
            Debug.Log("Tutu1 has been disabled at checkpoint!");
            TutusText.gameObject.SetActive(false);
            Debug.Log("Tutu1 text has been disabled at checkpoint!");
        }
    }
}
