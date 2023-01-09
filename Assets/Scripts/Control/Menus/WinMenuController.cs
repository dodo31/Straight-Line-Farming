using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinMenuController : MenuController
{
    [SerializeField]
    private TMP_Text dayCountText;

    [SerializeField]
    private TMP_Text dayCountTextShadow;

    public void SetDayCount(int dayCount)
    {
        dayCountText.text = $"I won in {dayCount.ToString()} days !";
        dayCountTextShadow.text = dayCountText.text;
    }
}
