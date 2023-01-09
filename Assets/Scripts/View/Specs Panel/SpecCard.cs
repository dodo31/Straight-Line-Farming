using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecCard : MonoBehaviour
{
    private const float ANIMATIONS_SPEED = 0.2f;
    public const float HIDDEN_POS_X_MARGIN = 10;
    public const float IDLE_POS_X_MARGIN = 5;
    
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

    private SpecCardStates cardState;

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
        targetPosX = 0;
        targetPosY = 0;

        cardState = SpecCardStates.IDLE;
    }

    public void SetSpec(Spec spec)
    {
        this.spec = spec;

        SetClientName(spec.ClientName);
        SetClientIcon(spec.ClientSprite);
        SetDeadline(spec.Deadline);
        SetGain(spec.Gain);
    }
    public void DecreaseDeadline()
    {
        spec.DecreaseDeadline();
        SetDeadline(spec.Deadline);
        if(spec.Deadline <= 0)
        {
            Validate();
            ShopVars.GetInstance().lives--;
        }
    }

    public void ResetDeadline()
    {
        spec.ResetDeadline();
        SetDeadline(spec.Deadline);
    }

    public void SetAsNormal()
    {
        if (!IsLeaving)
        {
            RectTransform parentTransform = (RectTransform)transform.parent;
            targetPosX = IDLE_POS_X_MARGIN;
        }
    }

    public void SetAsPreview()
    {
        if (!IsLeaving)
        {
            RectTransform parentTransform = (RectTransform)transform.parent;
            targetPosX = -35;
        }
    }

    public void UpdatePosX()
    {
        float deltaX = targetPosX - rectTransform.anchoredPosition.x;

        if (Math.Abs(deltaX) > 0.5f)
        {
            float newPosX = rectTransform.anchoredPosition.x + deltaX * ANIMATIONS_SPEED;
            rectTransform.anchoredPosition = new Vector2(newPosX, rectTransform.anchoredPosition.y);
        }
        else
        {
            switch (cardState)
            {
                case SpecCardStates.LEAVING:
                    cardState = SpecCardStates.LEAVED;
                    break;
            }
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

        targetPosX = rectTransform.sizeDelta.x + SpecCard.HIDDEN_POS_X_MARGIN;
        cardState = SpecCardStates.LEAVING;
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
        plantCounIndicator.SetPlantIcon(plantDescription.UiSprite);
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

    public bool IsLeaving
    {
        get
        {
            return cardState == SpecCardStates.LEAVING;
        }
    }

    public bool HasLeaved
    {
        get
        {
            return cardState == SpecCardStates.LEAVED;
        }
    }

    public Spec Spec { get => spec; }

    public float OrderTargetPosX { set => targetPosX = value; }
    public float TargetPosY { set => targetPosY = value; }
}