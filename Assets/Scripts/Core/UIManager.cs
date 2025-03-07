using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardCanvas;
    [SerializeField] private AudioSource buttonClickSound;

    //load menu
    public void LoadMenu()
    {
        PlayButtonSound();
        LoadScene("Menu");
    }

    //load controls
    public void LoadControls()
    {
        PlayButtonSound();
        LoadScene("Controls");
    }
    //load game scene
    public void LoadGame()
    {
        PlayButtonSound();
        LoadScene("Controls");
    }
    //load leaderboard
    public void ShowLeaderboard()
    {
        PlayButtonSound();
        if (leaderboardCanvas != null)
        {
            leaderboardCanvas.SetActive(true);
        }
    }
    //hide leaderboard
    public void HideLeaderboard()
    {
        PlayButtonSound();
        if (leaderboardCanvas != null)
        {
            leaderboardCanvas.SetActive(false);
        }
    }
    //quit game
    public void QuitGame()
    {
        PlayButtonSound();
        Application.Quit();
    }

    //playbutton sound
    private void PlayButtonSound()
    {
        if (buttonClickSound
             != null)
        {
            buttonClickSound.Play();
        }
    }
    //load scene
    private void LoadScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}
