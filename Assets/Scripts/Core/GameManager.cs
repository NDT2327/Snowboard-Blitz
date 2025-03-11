using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Transform startPoint;//điểm bắt đầu
    //điểm kết thúc
    [SerializeField] private Transform endPoint;
    //người chơi
    [SerializeField] private Transform player;


    //Hiển thị khoảng cách còn lại
    [SerializeField] private TMP_Text distanceText;
    //Hiển thị thời gian chơi
    [SerializeField] private TMP_Text timerText;
    //Hiển thị tốc độ di chuyển của người chơi
    [SerializeField] private TMP_Text speedText;
    //Hiển thị điểm số 
    [SerializeField] private TMP_Text scoreText;

    //game over GO
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private Text gameOverScoreText;

    //Congratulation GO
    [SerializeField] private GameObject congratulationCanvas;
    [SerializeField] private Text congratulationText;


    private float startTime;
    private bool isGameRunning = true;
    private Rigidbody2D playerRb;
    private float currentScore = 0;


    void Start()
    {
        Time.timeScale = 1; // Đảm bảo game không bị pause
        //lưu thời gian bắt đầu game
        startTime = Time.time;

        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning)
        {
            UpdateDistance();
            UpdateTimer();
            UpdateSpeed();
            UpdateScoreText();
            CheckGameEnd();
        }
    }

    //cập nhật khoảng cách từ người chơi đến EndPoint
    void UpdateDistance()
    {
        if (player == null) return;
        float remainingDistance = Vector2.Distance(player.position, endPoint.position);
        distanceText.text = remainingDistance.ToString("F1") + "m";
    }

    //cập nhật thời gian chơi
    void UpdateTimer()
    {
        if (player == null) return;
        float elapsedTime = Time.time - startTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = $"Time: {minutes: 00} seconds: {seconds:00}";
    }

    void UpdateSpeed()
    {
        if (playerRb != null)
        {
            float speed = playerRb.linearVelocity.magnitude;
            speedText.text = $"Speed: {speed:F1} m/s";
        }
    }

    public void UpdateScore(float score)
    {
        currentScore = score;
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString("F0");
        }
    }

    public void GameOver()
    {
        AudioManager.instance.PlayGameOver();
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;//stop game
        if (gameOverScoreText != null)
        {
            gameOverScoreText.text = "Your score: " + currentScore.ToString("F0");
        }

    }

    public void Congratulation()
    {
        AudioManager.instance.PlayWinSound();
        congratulationCanvas.SetActive(true);
        Time.timeScale = 0;//stop game



        if (congratulationText != null)
        {
            congratulationText.text = "Your score: " + currentScore.ToString("F0");
        }
    }

    public void RestartGame()
    {
        AudioManager.instance.PlayButtonSound();
        gameOverCanvas.SetActive(false);
        Time.timeScale = 1;//continue
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //find the player
        StartCoroutine(FindPlayerAfterSceneLoad());

        AudioManager.instance.RestartGameBGMusic();
    }

    public void BackToMainMenu()
    {
        AudioManager.instance.PlayButtonSound();
        SceneManager.LoadScene("Menu");
    }

    //Dừng game khi đến điểm kết thúc
    public void CheckGameEnd()
    {
        if (player == null) return; // Không làm gì nếu player đã bị hủy

        if (Vector2.Distance(player.position, endPoint.position) < 1f)
        {
            isGameRunning = false;
            Congratulation();
        }
    }

    // Coroutine để đợi scene load xong trước khi tìm player
    private IEnumerator FindPlayerAfterSceneLoad()
    {
        yield return new WaitForSeconds(0.1f); // Chờ 1 frame để đảm bảo scene đã load xong

        player = GameObject.FindWithTag("Player")?.transform;
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }
    }
}
