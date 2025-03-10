using System;
using System.Collections;
using Unity.IntegerTime;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private int score;

    Rigidbody2D rb2d;

    private int jumpCount = 0;
    private bool isGrounded = true;//kiểm tra xem có chạm đát
    private float defaultGravityScale;
    private float currentStamina;
    private GameManager gameManager;
    private ParticleSystem snowTrails;


    // Thêm âm thanh
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip groundCollision;
    [SerializeField] private AudioClip snowSlider;
    private AudioSource audioSource;
    // Khai báo thêm một AudioSource cho âm thanh trượt tuyết
    private AudioSource snowAudioSource;


    // Rotation tracking
    private float lastRotation = 0f;
    private float totalRotation = 0f; // Tổng góc xoay tích lũy trong không trung
    private int fullRotations = 0; // Số vòng xoay hoàn chỉnh
    private float startTime;
    private bool mapCompleted = false;

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


        // Khởi tạo AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        // Tạo một AudioSource riêng cho âm thanh trượt tuyết
        snowAudioSource = gameObject.AddComponent<AudioSource>();
        snowAudioSource.clip = snowSlider;
        snowAudioSource.loop = false; // Lặp lại liên tục
        snowAudioSource.playOnAwake = false; // Không phát ngay khi bắt đầu

        lastRotation = transform.eulerAngles.z;
        startTime = Time.time;


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

            // Phát âm thanh khi nhảy
            PlaySound(jumpSound);

            totalRotation = 0f; // Reset totalRotation khi nhảy
            fullRotations = 0;  // Reset fullRotations khi nhảy
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

        if (isGrounded)
        {
            if (rb2d.linearVelocity.magnitude > 0.5f) // Chỉ phát âm thanh khi tốc độ đủ lớn
            {
                if (!snowAudioSource.isPlaying)
                {
                    snowAudioSource.Play();
                }
            }
            else
            {
                if (snowAudioSource.isPlaying)
                {
                    snowAudioSource.Stop();
                }
            }
        }
        else
        {
            if (snowAudioSource.isPlaying)
            {
                snowAudioSource.Stop();
            }
        }

        TrackRotation();

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

    private void TrackRotation()
    {
        float currentRotation = transform.eulerAngles.z;
        float rotationDelta = Mathf.DeltaAngle(lastRotation, currentRotation);

        // Chỉ theo dõi xoay khi ở trên không
        if (!isGrounded)
        {
            totalRotation += rotationDelta; // Tích lũy tổng góc xoay

            // Kiểm tra nếu hoàn thành một vòng xoay (360 độ)
            while (Mathf.Abs(totalRotation) >= 360f)
            {
                fullRotations += (int)Mathf.Sign(totalRotation); // +1 hoặc -1 tùy hướng xoay
                totalRotation -= Mathf.Sign(totalRotation) * 360f; // Giảm đi 360 độ đã tính
                Debug.Log($"In Air - Full Rotation Detected! Total Rotations: {fullRotations}, Remaining Rotation: {totalRotation}");
            }
        }

        lastRotation = currentRotation;
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

    public void CompleteMap()
    {
        if (mapCompleted) return;//If completed no update score
        mapCompleted = true;
        float completion = Time.time - startTime;

        if (completion < 60)
        {
            score += 500;
            Debug.Log("Time Bonus! TOtal score: " + score);
        }
        gameManager.UpdateScore(score);
        Debug.Log("Final score: " + score);
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

                    PlaySound(groundCollision);

                    // Đáp đất thành công, tính điểm nếu có xoay
                    if (Mathf.Abs(fullRotations) >= 1)
                    {
                        score += 100 * Mathf.Abs(fullRotations);
                        gameManager.UpdateScore(score);
                        Debug.Log($"Landed Successfully! Rotations: {fullRotations}, Score: {score}");
                    }
                    totalRotation = 0f; // Reset sau khi đáp đất
                    fullRotations = 0;

                }
            }
        }
        else if (collision.gameObject.CompareTag("EndFlag"))
        {
            // Kiểm tra nếu Scene hiện tại là "Game"
            if (SceneManager.GetActiveScene().name == "Game")
            {
                ShowCongratulation();
            }
        }
        else
        {   

            PlaySound(crashSound);
            StartCoroutine(GameOverDelay());
        }
    }

    private IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(0.5f); // Chờ 2 giây trước khi kết thúc game
        StopGame();
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
        // Dừng mọi âm thanh đang phát
        audioSource.Stop();
        snowAudioSource.Stop();

        // Nếu có nhiều AudioSource trong game, dừng tất cả
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allAudioSources)
        {
            source.Stop();
        }

        // Gọi hàm kết thúc game từ GameManager
        gameManager.GameOver();
    }

    public void ShowCongratulation()
    {
        // Dừng mọi âm thanh đang phát
        audioSource.Stop();
        snowAudioSource.Stop();

        // Nếu có nhiều AudioSource trong game, dừng tất cả
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allAudioSources)
        {
            source.Stop();
        }

        // Gọi hàm kết thúc game từ GameManager
        gameManager.Congratulation();
    }


    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
