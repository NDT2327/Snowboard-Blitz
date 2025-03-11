using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    // Tham chiếu đến các nút trong Inspector
    [SerializeField] private Button playButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button backButton; // Nếu có nút Back trong scene Tutorial

    void Start()
    {
        // Gán sự kiện onClick cho các nút
        if (playButton != null)
            playButton.onClick.AddListener(LoadGame);

        if (tutorialButton != null)
            tutorialButton.onClick.AddListener(LoadControls);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        if (backButton != null)
            backButton.onClick.AddListener(BackToMainMenu);
    }
    //load menu
    public void LoadMenu()
    {
        AudioManager.instance.PlayButtonSound();
        LoadScene("Menu");
    }

    //load controls
    public void LoadControls()
    {
        AudioManager.instance.PlayButtonSound();

        LoadScene("Control");
    }
    //load game scene
    public void LoadGame()
    {
        AudioManager.instance.PlayButtonSound();

        LoadScene("Game 1");
    }

    public void BackToMainMenu()
    {
        AudioManager.instance.PlayButtonSound();
        LoadScene("Menu");
    }

    //quit game
    public void QuitGame()
    {
        AudioManager.instance.PlayButtonSound();

        Application.Quit();
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
