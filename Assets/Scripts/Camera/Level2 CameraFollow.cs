using UnityEngine;
using System.Collections;

public class Level2CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(2, 3, 0);

    public bool followX = true;
    public bool followY = true;
    public bool followZ = false; // -5.190003 -0.1665073 -10

    IEnumerator StartCamera()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.position + offset;

        float elapsedTime = 0f;
        float duration = 1.5f;  // Duration of the transition

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;  // Ensure final position is correct
    }

    void Start()
    {
        transform.position = new Vector3(-25.19115f, -0.166f, -10f);  // Set to desired position
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Apply axis restrictions
        Vector3 smoothedPosition = transform.position;

        if (followX)
            smoothedPosition.x = Mathf.Lerp(transform.position.x, desiredPosition.x, smoothSpeed * Time.deltaTime);
        if (followY)
            smoothedPosition.y = Mathf.Lerp(transform.position.y, desiredPosition.y, smoothSpeed * Time.deltaTime);
        if (followZ)
            smoothedPosition.z = Mathf.Lerp(transform.position.z, desiredPosition.z, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
