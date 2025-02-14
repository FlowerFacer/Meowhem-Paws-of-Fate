using UnityEngine;
using System.Collections;
using TMPro;

public class TutuPlush : MonoBehaviour
{
    public GameObject TutuCollectible; // Assign this in the Inspector
    public Transform plushThrowPoint; // Empty GameObject to set throw position
    public float throwForce = 7f; // Adjust the throw strength

    public BossTutu3 bossTutu; // Reference to the boss script
    public GameObject tutuBoss; // Reference to the actual boss GameObject
    private bool hasThrown = false;

    void Start()
    {
        // Find the boss if not assigned
        if (bossTutu == null)
        {
            bossTutu = FindFirstObjectByType<BossTutu3>();
        }

        // Start checking if boss is destroyed
        StartCoroutine(CheckBossStatus());
    }

    IEnumerator CheckBossStatus()
    {
        while (tutuBoss != null)
        {
            yield return null; // Wait until the boss is destroyed
        }

        if (!hasThrown)
        {
            hasThrown = true;
            TutuIsDead();
        }
    }

    public void TutuIsDead()
    {
        if (TutuCollectible != null && plushThrowPoint != null)
        {
            GameObject thrownPlush = Instantiate(TutuCollectible, plushThrowPoint.position, Quaternion.identity);

            // ✅ Ensure plush does NOT flip when instantiated
            thrownPlush.transform.rotation = Quaternion.identity; // ✅ Reset rotation

            Rigidbody2D plushRB = thrownPlush.GetComponent<Rigidbody2D>();
            if (plushRB != null)
            {
                plushRB.linearVelocity = new Vector2(-throwForce, 3.5f); // Apply force to "throw" it
            }

            // ✅ Fix the text flipping by setting its local scale
            Transform textTransform = thrownPlush.transform.GetComponentInChildren<TMP_Text>().transform;
            if (textTransform != null)
            {
                textTransform.localScale = new Vector3(1, 1, 1); // Reset scale to normal
            }

            Debug.Log("🍄 Tutu plush has been dropped to the player!");
        }
        else
        {
            Debug.LogError("❌ Tutu prefab or throw point is not assigned!");
        }
    }
}
