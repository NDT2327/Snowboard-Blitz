using UnityEngine;
using UnityEngine.SceneManagement;

public class Game1Event : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name; // Lấy tên scene hiện tại

        if (currentSceneName == "Game 1")
        {
            SceneManager.LoadSceneAsync("Game"); // Chuyển về scene "Game"
        }
        else
        {
            Debug.LogWarning("Tên scene không khớp với 'Game' hoặc 'Game 1'.");
        }
    }
}
