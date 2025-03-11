using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float deadZone = -26f;
    public GameObject explosionPrefab;
    private AudioSource audioSource;

    // Các biến cấu hình cho chuyển đổi giữa bay thẳng và bay zigzag
    public float cycleDuration = 3.0f;      // Tổng thời gian của một chu kỳ (ví dụ: 3 giây)
    public float zigzagDuration = 1.5f;     // Thời gian bay zigzag trong mỗi chu kỳ (ví dụ: 1.5 giây)
    public float zigzagAmplitude = 3.0f;    // Biên độ chuyển động lên xuống của zigzag
    public float zigzagFrequency = 5.0f;    // Tần số của chuyển động zigzag
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        // Tính thời gian đã trôi qua trong chu kỳ hiện tại
        float timeInCycle = Time.time % cycleDuration;

        // Di chuyển cơ bản sang bên trái
        Vector3 movement = Vector3.left * moveSpeed * Time.deltaTime;

        // Nếu thời gian trong chu kỳ nhỏ hơn zigzagDuration, áp dụng chuyển động zigzag
        if (timeInCycle < zigzagDuration)
        {
            // Sử dụng hàm sin để tạo chuyển động lên xuống mượt mà
            float verticalOffset = Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude * Time.deltaTime;
            movement += Vector3.up * verticalOffset;
        }

        // Cập nhật vị trí của con chim
        transform.Translate(movement);

        if (transform.position.x < deadZone)
        {
            Debug.Log("Bird remove");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // Hiển thị hiệu ứng nổ nếu có
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, 0.5f); // Xóa hiệu ứng sau 0.5 giây
            }
            audioSource.Play();
            //Destroy(audioSource, audioSource.clip.length);

            GetComponent<SpriteRenderer>().enabled = false; // Ẩn sprite ngay khi va chạm
            GetComponent<Collider2D>().enabled = false; // Vô hiệu hóa collider tránh va chạm tiếp
            gameObject.SetActive(false);

        }
    }
}
