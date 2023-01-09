using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static int daysToWin = 0;
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
