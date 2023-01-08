using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionItem : MonoBehaviour
{
    [SerializeField]
    private Button itemButton;

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private UserAction targetAction;

    private bool isSelected;

    public event Action<ActionItem> OnClicked;

    protected void Awake()
    {
        isSelected = false;
    }

    protected void Start()
    {
        itemButton.onClick.AddListener(() =>
        {
            OnClicked?.Invoke(this);
        });
    }

    public void Select()
    {
        isSelected = true;
    }

    public void Unselect()
    {
        isSelected = false;
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

    public void SetTargetAction(UserAction action)
    {
        targetAction = action;
    }

    public UserAction TargetAction { get => targetAction; }
    public bool IsSelected { get => isSelected; }
}