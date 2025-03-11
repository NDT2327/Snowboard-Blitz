using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
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
