using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public abstract class ActionButton : MonoBehaviour
{
    protected Image image;

    protected void Awake()
    {
        image = GetComponent<Image>();
    }
    
    public Image Image { get => image; set => image = value; }
}