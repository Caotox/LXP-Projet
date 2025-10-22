using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 5f;

    public Vector3 offset;

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
