using UnityEngine;

public class TutuCheckpointDisabler : MonoBehaviour
{
    public GameObject tutu1; // Assign Tutu1 GameObject in the Inspector

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && tutu1 != null) // If the player reaches the checkpoint
        {
            tutu1.SetActive(false); // Disable Tutu1 to stop talking
            Debug.Log("Tutu1 has been disabled at checkpoint!");
        }
    }
}
