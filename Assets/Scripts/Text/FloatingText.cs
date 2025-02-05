using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Transform character;
    public Vector3 offset = new Vector3(0, 1f, 0); // Adjust height of text

    void Update()
    {
        if (character != null)
        {
            transform.position = character.position + offset;
        }
    }
}
