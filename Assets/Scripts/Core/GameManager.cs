﻿using TMPro;
using UnityEditor.SearchService;
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

    //sound
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip buttonClickSound;

    private float startTime;
    private bool isGameRunning = true;
    private Rigidbody2D playerRb;
    private float currentScore = 0;


    void Start()
    {
        //lưu thời gian bắt đầu game
        startTime = Time.time;
        playerRb = player.GetComponent<Rigidbody2D>();
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
        float remainingDistance = Vector2.Distance(player.position, endPoint.position);
        distanceText.text = remainingDistance.ToString("F1") + "m";
    }

    //cập nhật thời gian chơi
    void UpdateTimer()
    {
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
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;//stop game

        if (gameOverScoreText != null) {
            gameOverScoreText.text ="Your score: " + currentScore.ToString("F0");
        }

        if (audioSource != null && gameOverSound != null) {
            audioSource.PlayOneShot(gameOverSound);
        }
    }

    public void PlayButtonSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    public void RestartGame()
    {
        PlayButtonSound();
        gameOverCanvas.SetActive(false);
        Time.timeScale = 1;//continue
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void BackToMainMenu()
    {
        PlayButtonSound();
        SceneManager.LoadScene("Menu");
    }

    //Dừng game khi đến điểm kết thúc
    public void CheckGameEnd()
    {
        if (Vector2.Distance(player.position, endPoint.position) < 1f)
        {
            isGameRunning = false;
            Debug.Log("Congratulations");
        }
    }
}
