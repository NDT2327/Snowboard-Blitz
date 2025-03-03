using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Nhân vật cần theo dõi
    public float smoothSpeed = 0.1f;
    public Vector3 offset = new Vector3(0, 1, -10); // Đặt camera đúng vị trí

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
    }
}
