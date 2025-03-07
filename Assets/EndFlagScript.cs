using UnityEngine;
using UnityEngine.SceneManagement; // Để load màn mới

public class EndFlagScript : MonoBehaviour
{

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Kiểm tra nếu nhân vật chạm vào
        {
            Game1Event game1Event = FindFirstObjectByType<Game1Event>();
            if (game1Event != null)
            {
                game1Event.LoadNextLevel(); // Gọi hàm LoadNextLevel()
            }
            else
            {
                Debug.LogError("Không tìm thấy Game1Event trong scene!");
            }
        }
    }

}
