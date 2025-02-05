using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 respawnPoint;

    void Start()
    {
        respawnPoint = transform.position; // Set the initial respawn point
    }

    public void UpdateCheckpoint(Vector3 newCheckpoint)
    {
        respawnPoint = newCheckpoint;
        Debug.Log("Checkpoint Updated! New Respawn Point: " + respawnPoint);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DeathZone")) // If player falls into a gap
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        Debug.Log("Player Respawned at: " + respawnPoint);
        transform.position = respawnPoint; // Move player to last checkpoint
    }
}
