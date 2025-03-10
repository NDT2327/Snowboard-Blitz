using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
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
        LoadScene("Control");
    }
    //load game scene
    public void LoadGame()
    {
        PlayButtonSound();
        LoadScene("Game 1");
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
