using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public Screen MainMenuScreen;

    protected void Awake()
    {
        MainMenuScreen.Toggle(true);
    }

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
