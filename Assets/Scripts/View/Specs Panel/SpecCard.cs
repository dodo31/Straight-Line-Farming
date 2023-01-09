using System.Net.Mail;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecCard : MonoBehaviour
{
    [SerializeField]
    private const float ANIMATIONS_SPEED = 0.25f;
    
    [SerializeField]
    private TMP_Text clientText;

    [SerializeField]
    private Image clientImage;

    [SerializeField]
    private Transform plantCountIndicatorContainer;

    [SerializeField]
    private TMP_Text deadlineValueText;

    [SerializeField]
    private TMP_Text deadlineUnitText;

    [SerializeField]
    private TMP_Text gainValueText;

    [SerializeField]
    private TMP_Text gainUnitText;

    [SerializeField]
    private Image backgroundImage;

    private Spec spec;

    private RectTransform rectTransform;
    private float targetPosX;
    private float targetPosY;

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
        targetPosX = 0;
        targetPosY = 0;
    }

    public void SetSpec(Spec spec)
    {
        this.spec = spec;

        SetClientName(spec.ClientName);
        SetClientIcon(Resources.Load<Sprite>(spec.ClientSpritePath));
        SetDeadline(spec.Deadline);
        SetGain(spec.Gain);
    }

    public void SetAsNormal()
    {
        RectTransform parentTransform = (RectTransform)transform.parent;
        targetPosX = -rectTransform.sizeDelta.x / 2f;
    }

    public void SetAsPreview()
    {
        RectTransform parentTransform = (RectTransform)transform.parent;
        targetPosX = -rectTransform.sizeDelta.x / 2f - 35;
    }

    public void UpdatePosX()
    {
        float deltaX = targetPosX - rectTransform.anchoredPosition.x;

        if (Math.Abs(deltaX) > 0.5f)
        {
            float newPosX = rectTransform.anchoredPosition.x + deltaX * ANIMATIONS_SPEED;
            rectTransform.anchoredPosition = new Vector2(newPosX, rectTransform.anchoredPosition.y);
        }
    }

    public void UpdatePosY()
    {
        float deltaY = targetPosY - rectTransform.anchoredPosition.y;

        if (Math.Abs(deltaY) > 0.5f)
        {
            float newPosY = rectTransform.anchoredPosition.y + deltaY * ANIMATIONS_SPEED;
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newPosY);
        }
    }

    public void Validate()
    {
        Debug.Log($"Panel was validated! {spec.ClientName} is happy!");

        Debug.Log(gameObject);

        DestroyImmediate(gameObject);
    }

    public void SetClientName(string clientName)
    {
        clientText.text = clientName;
    }

    public void SetClientIcon(Sprite clientIcon)
    {
        clientImage.sprite = clientIcon;
    }

    public void AddRequiredPlantCount(PlantDescription plantDescription, PlantCountIndicator plantCounIndicator, int plantCount)
    {
        plantCounIndicator.transform.SetParent(plantCountIndicatorContainer);
        plantCounIndicator.SetPlantType(plantDescription.Type);
        plantCounIndicator.SetPlantIcon(plantDescription.GridSprite);
        plantCounIndicator.SetPlantCount(plantCount);
    }

    public void SetDeadline(int deadline)
    {
        deadlineValueText.text = deadline.ToString();

        if (deadline > 1)
        {
            deadlineUnitText.text = "Days";
        }
        else
        {
            deadlineUnitText.text = "Day";
        }
    }

    public void SetGain(float gainRaw)
    {
        float gain = Mathf.Round(gainRaw);
        gainValueText.text = gain.ToString();
    }

    public Spec Spec { get => spec; }

    public float OrderTargetPosX { set => targetPosX = value; }
    public float TargetPosY { set => targetPosY = value; }
}