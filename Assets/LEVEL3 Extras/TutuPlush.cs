using UnityEngine;
using System.Collections;

public class TutuPlush : MonoBehaviour
{
    public GameObject TutuCollectible; // Assign this in the Inspector
    public Transform plushThrowPoint; // Empty GameObject to set throw position
    public float throwForce = 7f; // Adjust the throw strength

    public BossTutu3 bossTutu; // Reference to the boss script
    public GameObject tutuBoss; // Reference to the actual boss GameObject

    void Start()
    {
        // Find the boss if not assigned
        if (bossTutu == null)
        {
            bossTutu = FindFirstObjectByType<BossTutu3>();
        }
    }

    void Update()
    {
        // **Stop lightning when boss is defeated**
        if (tutuBoss == null)
        {
            TutuIsDead();
        }
    }

    public void TutuIsDead()
    {
        if (TutuCollectible != null && plushThrowPoint != null)
        {
            GameObject thrownPlush = Instantiate(TutuCollectible, plushThrowPoint.position, Quaternion.identity);

            Rigidbody2D plushRB = thrownPlush.GetComponent<Rigidbody2D>();
            if (plushRB != null)
            {
                plushRB.linearVelocity = new Vector2(throwForce, 3.5f); // Apply force to "throw" it
            }

            Debug.Log("🍄 Tutu plush has been dropped to the player!");
        }
        else
        {
            Debug.LogError("❌ Tutu prefab or throw point is not assigned!");
        }
    }
}
