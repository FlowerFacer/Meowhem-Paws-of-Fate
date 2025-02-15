using UnityEngine;
using System.Collections;

public class HealthBar2D : MonoBehaviour
{
    public Transform healthFill; // The green bar
    public Transform target; // The player/enemy it follows
    public SpriteRenderer healthBarRenderer; // Sprite Renderer for visibility
    public Vector3 offset = new Vector3(0, 1.5f, 0); // Adjust position above character

    private float maxScaleX; // Store initial width
    private Coroutine hideCoroutine; // Store fade-out coroutine

    void Start()
    {
        maxScaleX = healthFill.localScale.x; // Store the max size

        // **Start Hidden**
        if (healthBarRenderer != null)
        {
            healthBarRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset; // Follow character
        }
    }

    public void UpdateHealth(float currentHP, float maxHP)
    {
        float healthPercent = currentHP / maxHP;
        healthFill.localScale = new Vector3(maxScaleX * healthPercent, healthFill.localScale.y, 1);

        // **Show Health Bar**
        if (healthBarRenderer != null)
        {
            healthBarRenderer.enabled = true;
        }

        // **Reset Fade Coroutine**
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        hideCoroutine = StartCoroutine(HideHealthBar());
    }

    IEnumerator HideHealthBar()
    {
        yield return new WaitForSeconds(3f); // Wait before fading out

        if (healthBarRenderer != null)
        {
            healthBarRenderer.enabled = false;
        }
    }
}
