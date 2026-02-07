using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform pivot;   // usually head / camera pivot
    public float smoothSpeed = 12f;

    void LateUpdate()
    {
        if (!pivot) return;

        // stick camera to pivot
        transform.position = Vector3.Lerp(
            transform.position,
            pivot.position,
            smoothSpeed * Time.deltaTime
        );

        // match rotation exactly
        transform.rotation = pivot.rotation;
    }
}
