using UnityEngine;

public class BossWallController : MonoBehaviour
{
    public GameObject tutuBoss; // Assign the boss in the Inspector
    public BoxCollider2D wallCollider;

    void Start()
    {
        wallCollider = GetComponent<BoxCollider2D>();

        if (tutuBoss == null)
        {
            Debug.LogError("⚠️ TutuBoss reference is missing! Assign it in the Inspector.");
        }
    }

    void Update()
    {
        // If the boss is destroyed (null), disable the collider
        if (tutuBoss == null && wallCollider != null)
        {
            wallCollider.enabled = false;
            Debug.Log("✅ Boss defeated! Invisible wall removed.");
            Destroy(this); // Remove script since it's no longer needed
        }
    }
}
