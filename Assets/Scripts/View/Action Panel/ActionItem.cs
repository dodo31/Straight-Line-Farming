using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class ActionItem : MonoBehaviour
{

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private Text titleText;

    [SerializeField]
    private Image backgroundImage;

    protected void Awake()
    {

    }

    public void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
    }

    public void SetTile(string text)
    {
        titleText.text = text;
    }

    public void SetBackgroundColor(Color color)
    {
        backgroundImage.color = color;
    }
}