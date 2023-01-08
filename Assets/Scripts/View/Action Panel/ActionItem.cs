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
    private Image selectionBackgroundImage;

    [SerializeField]
    private UserAction targetAction;

    [SerializeField]
    private bool isAvailable;

    private bool isSelected;

    public event Action<ActionItem> OnClicked;

    protected void Awake()
    {
        isAvailable = false;
        isSelected = false;

        ToggleAvailability(isAvailable);
        ToggleSelection(isSelected);
    }

    protected void Start()
    {
        itemButton.onClick.AddListener(() =>
        {
            OnClicked?.Invoke(this);
        });
    }

    private void ToggleAvailability(bool setAvailable)
    {
        if (!setAvailable)
        {
            Unselect();
        }
        else
        {
            Select();
        }
    }

    public void SetUnavailable()
    {
        itemButton.interactable = false;
        titleText.color = new Color(1, 1, 1, 0.5f);
        iconImage.color = new Color(1, 1, 1, 0.5f);
        isAvailable = false;
    }

    public void SetAvailable()
    {
        itemButton.interactable = true;
        titleText.color = Color.white;
        iconImage.color = Color.white;
        isAvailable = true;
    }

    private void ToggleSelection(bool setSelected)
    {
        if (!setSelected)
        {
            Unselect();
        }
        else
        {
            Select();
        }
    }

    public void Unselect()
    {
        selectionBackgroundImage.enabled = false;
        isSelected = false;
    }

    public void Select()
    {
        selectionBackgroundImage.enabled = true;
        isSelected = true;
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