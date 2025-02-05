using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Checkpoint Activated at: " + transform.position);
            other.GetComponent<PlayerRespawn>().UpdateCheckpoint(transform.position);
        }
    }
}
