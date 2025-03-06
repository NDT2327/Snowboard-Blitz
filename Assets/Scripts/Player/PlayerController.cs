using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float boostMultiplier = 5f;//Hệ số tăng tốc
    [SerializeField] float staminaDecreaseRate = 20f; // Mức tiêu hao stamina khi Boost
    [SerializeField] float staminaRecoveryRate = 10f; // Mức hồi phục stamina mỗi giây
    [SerializeField] float maxStamina = 100f; // Giá trị stamina tối đa
    [SerializeField] Slider staminaBar; // UI thanh stamina
    [SerializeField] Image staminaFill;

    [SerializeField] public CircleCollider2D head;
    [SerializeField] public CircleCollider2D body;
    [SerializeField] public CapsuleCollider2D snowboard;
    Rigidbody2D rb2d;

    private int jumpCount = 0;
    private bool isGrounded = true;//kiểm tra xem có chạm đát
    private float defaultGravityScale;
    private float currentStamina;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb2d.gravityScale;
        currentStamina = maxStamina;

        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
        }
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
            //sau khi nhảy không còn chạm đất
            isGrounded = false;
        }

        //giữ phím sẽ tăng tốc
        //đảm bảo phải chạm đất để tăng tốc
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.S))
        {
            if (isGrounded && currentStamina > 0)
            {
                Debug.Log(isGrounded);
                Boost();
            }
        }
        else
        {
            //Hồi phục lại khi không boost
            RecoverStamina();
        }
    }

    private void Boost()
    {
        if (!isGrounded) return; // Chỉ tăng tốc khi đang chạm đất

        rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x + boostMultiplier * Time.deltaTime, rb2d.linearVelocity.y);
        currentStamina -= staminaDecreaseRate * Time.deltaTime;
        UpdateStaminaUI();

        if (currentStamina <= 0)
        {
            currentStamina = 0;
        }
    }

    private void RecoverStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            UpdateStaminaUI();
        }
    }

    private void UpdateStaminaUI()
    {
        if (staminaBar != null)
        {
            staminaBar.value = currentStamina;
        }
    }


    // Hàm xử lý va chạm
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu đối tượng va chạm có tag "Ground"
        if (collision.gameObject.CompareTag("Ground") )
        {
            // Lặp qua các điểm va chạm để kiểm tra xem có điểm nào nằm trong vùng collider của head hay không
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (head.bounds.Contains(contact.point) || body.bounds.Contains(contact.point))
                {
                    StopGame();
                    break;
                }
                else if (snowboard.bounds.Contains(contact.point))
                {
                    jumpCount = 0;
                    isGrounded = true;
                }
            }
        }
        else
        {
            StopGame();
        }
    }

    //public void OnCollisionExit2D(Collision2D collision)
    //{
    //    // Kiểm tra nếu rời khỏi "Ground"
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = false; // Không còn tiếp xúc với mặt đất
    //    }
    //}

    public void StopGame()
    {
        // Đặt Time.timeScale = 0 để tạm dừng mọi hoạt động của game
        Time.timeScale = 0f;

        // Nếu cần, bạn có thể hiển thị UI "Game Over" hoặc thực hiện các xử lý khác tại đây.
    }
}
