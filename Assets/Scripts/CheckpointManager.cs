using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 respawnPoint; // Stores the last checkpoint position
    public Transform player; // Assign player in Inspector
    public LayerMask deathZoneLayer; // Assign the "Death Zone" layer

    void Start()
    {
        // Set the first respawn point at the starting position
        respawnPoint = player.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint")) // Player reaches a checkpoint
        {
            respawnPoint = other.transform.position; // Update the respawn position
            Debug.Log("Checkpoint Reached! Respawn set to: " + respawnPoint);
        }

        if (other.CompareTag("DeathZone")) // Player falls into a gap
        {
            RespawnPlayer();
        }
    }

    void RespawnPlayer()
    {
        Debug.Log("Player fell! Respawning...");
        player.position = respawnPoint; // Move player to last checkpoint
    }
}
