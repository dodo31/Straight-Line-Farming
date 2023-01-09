using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    [SerializeField]
    Image[] lives;
    int lastLives = 0;
    // Update is called once per frame
    void Update()
    {
        if (lastLives != ShopVars.GetInstance().lives)
        {
            lastLives = ShopVars.GetInstance().lives;
            for (int i = 0; i < lives.Length; i++)
            {
                lives[i].enabled = i < lastLives;
            }
        }
        if(lastLives <= 0)
        {
            SceneManager.LoadScene("Lose");
        }
    }
}
