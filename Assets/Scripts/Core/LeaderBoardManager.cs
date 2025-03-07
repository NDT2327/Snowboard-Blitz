using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] private Text highScoreText;
    void Start()
    {
        float highScore = PlayerPrefs.GetFloat("High Score: ", 0);
        highScoreText.text = "Your highest score: " + highScore.ToString("F0");
    }

    // Update is called once per frame
    public void BackToMainMenu()
    {
        gameObject.SetActive(false);
    }
}
