using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public float moveSpeed = 5f; // T?c ?? di chuy?n
    private Rigidbody2D rb;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Nh?n gi� tr? ??u v�o t? b�n ph�m (-1 tr�i, 1 ph?i)
        moveInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        // Di chuy?n nh�n v?t theo tr?c X
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}
