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
    private TMP_Text priceValueText;
    [SerializeField]
    private TMP_Text priceUnitText;

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
            Debug.Log("oh");
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
        priceValueText.color = new Color(0, 0, 0, 0.5f);
        priceUnitText.color = new Color(0, 0, 0, 0.5f);
        iconImage.color = new Color(1, 1, 1, 0.5f);
        isAvailable = false;
    }

    public void SetAvailable()
    {
        itemButton.interactable = true;
        priceValueText.color = Color.black;
        priceUnitText.color = Color.black;
        iconImage.color = Color.white;
        isAvailable = true;
    }
    public void SetAvailable(bool available)
    {
        if (available) SetAvailable();
        else SetUnavailable();
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

    public void SetPrice(int price)
    {
        priceValueText.text = price.ToString();
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