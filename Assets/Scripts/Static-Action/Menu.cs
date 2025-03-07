using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    // trở về Menu 
    public void GoToMenu() {
        SceneManager.LoadScene("Menu");
    }

// thoát game
    public void Quit() {
        Application.Quit();
    }

    // bắt đầu chơi - Chơi lại
    public void Play() {
        SceneManager.LoadScene("Game 1");
    }

    // Bảng điều khiển
    public void Control()
    {
        SceneManager.LoadScene("Control");
    }



}
