using System;
using System.Collections;
using Unity.IntegerTime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float torqueAmount = 1f;
    [SerializeField] float jumpForce = 5f;
    private float lastJumpTime;
    private float jumpCooldown = 0.2f; // Giới hạn thời gian giữa các lần nhảy

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
    private GameManager gameManager;
    private ParticleSystem snowTrails;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb2d.gravityScale;
        rb2d.gravityScale = 2f;
        currentStamina = maxStamina;

        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
        }

        gameManager = FindAnyObjectByType<GameManager>();
        snowTrails = GetComponentInChildren<ParticleSystem>();

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


        //if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        //{
        //    rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        //    jumpCount++;
        //    //sau khi nhảy không còn chạm đất
        //    isGrounded = false;
        //}
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2 && Time.time - lastJumpTime > jumpCooldown)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0);
            rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
            isGrounded = false;
            lastJumpTime = Time.time;
        }

        //giữ phím sẽ tăng tốc
        //đảm bảo phải chạm đất để tăng tốc
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.S))
        {
            if (isGrounded && currentStamina > 0)
            {
                Boost();
            }
        }
        else
        {
            //Hồi phục lại khi không boost
            RecoverStamina();
            //thêm lực tự nhiên khi xuống dốc
            ApplySlopeSpeed();
        }

        if (isGrounded)
        {
            Vector2 groundNormal = GetGroundNormal();
            float slopeAngle = Vector2.Angle(groundNormal, Vector2.up);
            if (slopeAngle > 5f) // Chỉ áp dụng khi dốc đủ nghiêng
            {
                Vector2 boostDirection = new Vector2(groundNormal.y, -groundNormal.x).normalized;
                rb2d.AddForce(boostDirection * slopeAngle * 0.1f, ForceMode2D.Force); // Tăng tốc dựa trên góc dốc
            }
        }

        //Điều khiển Particle System dựa trên trạng thái chạm đất
        if (snowTrails != null)
        {
            if (isGrounded)
            {
                if (!snowTrails.isPlaying)
                {
                    snowTrails.Play();
                }
            }
            else
            {
                if (snowTrails.isPlaying)
                {
                    snowTrails.Stop();
                }
            }
        }
    }

    private void Boost()
    {
        if (!isGrounded) return;

        Vector2 groundNormal = GetGroundNormal();
        Vector2 boostDirection = new Vector2(groundNormal.y, -groundNormal.x).normalized;
        rb2d.AddForce(boostDirection * boostMultiplier, ForceMode2D.Force);

        currentStamina -= staminaDecreaseRate * Time.deltaTime;
        if (currentStamina <= 0) currentStamina = 0;
        UpdateStaminaUI();
    }

    private void ApplySlopeSpeed()
    {
        if (!isGrounded) return;

        Vector2 groundNormal = GetGroundNormal();
        float slopeAngle = Vector2.Angle(groundNormal, Vector2.up);
        if (slopeAngle > 5f) // Chỉ áp dụng khi dốc đủ nghiêng
        {
            Vector2 slopeDirection = new Vector2(groundNormal.y, -groundNormal.x).normalized;
            float slopeFactor = Mathf.Sin(slopeAngle * Mathf.Deg2Rad); // Tính thành phần lực theo góc dốc
            rb2d.AddForce(slopeDirection * slopeFactor * 10f, ForceMode2D.Force); // Điều chỉnh hệ số nếu cần
        }
    }

    private Vector2 GetGroundNormal()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            return hit.normal; //Tra ve phap tuyen cua mat dat
        }
        return Vector2.up;
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
        if (collision.gameObject.CompareTag("Ground"))
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

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    public void StopGame()
    {
        // Đặt Time.timeScale = 0 để tạm dừng mọi hoạt động của game
        gameManager.GameOver();

        // Nếu cần, bạn có thể hiển thị UI "Game Over" hoặc thực hiện các xử lý khác tại đây.
    }
}
