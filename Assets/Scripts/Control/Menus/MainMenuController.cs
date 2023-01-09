using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MenuController
{
    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
