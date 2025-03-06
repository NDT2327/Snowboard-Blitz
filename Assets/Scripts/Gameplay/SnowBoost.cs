using UnityEngine;

public class SnowBoost : MonoBehaviour
{
    private SurfaceEffector2D surfaceEffector;

    void Start()
    {
        // Tìm Surface Effector trên Terrain
        surfaceEffector = GetComponent<SurfaceEffector2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.V)) // Nhấn Space để tăng tốc
        {
            surfaceEffector.speed = 12f;
        }
        else
        {
            surfaceEffector.speed = 10f;
        }
    }

    bool OnSnowSurface()
    {
        // Kiểm tra nếu người chơi đang trượt trên tuyết bằng LayerMask
        return Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Snow"));
    }
}
