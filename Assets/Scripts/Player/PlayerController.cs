using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] public CircleCollider2D head;
    [SerializeField] public CapsuleCollider2D snowboard;
    Rigidbody2D rb2d;

    private int jumpCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb2d.AddTorque(torqueAmount);
        }
        else
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb2d.AddTorque(-torqueAmount);
        }

        
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    // Hàm xử lý va chạm
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu đối tượng va chạm có tag "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Lặp qua các điểm va chạm để kiểm tra xem có điểm nào nằm trong vùng collider của head hay không
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (head.bounds.Contains(contact.point))
                {
                    StopGame();
                    break;
                }
                else if(snowboard.bounds.Contains(contact.point))
                {
                    jumpCount = 0;
                }    
            }
        }
    }

    public void StopGame()
    {
        Debug.Log("Phần đầu chạm đất. Dừng game.");
        // Đặt Time.timeScale = 0 để tạm dừng mọi hoạt động của game
        Time.timeScale = 0f;

        // Nếu cần, bạn có thể hiển thị UI "Game Over" hoặc thực hiện các xử lý khác tại đây.
    }
}
