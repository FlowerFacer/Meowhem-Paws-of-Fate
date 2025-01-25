using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    public bool followX = true;
    public bool followY = true;
    public bool followZ = false;

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
