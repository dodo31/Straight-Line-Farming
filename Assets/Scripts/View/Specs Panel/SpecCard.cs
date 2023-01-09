using System.Net.Mail;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecCard : MonoBehaviour
{
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

    [SerializeField]
    private Animator cardAnimator;

    private RectTransform rectTransform;
    private float basePosX;

    [SerializeField]
    private float previewOffsetX = 35;

    private CardPreviewStates previewState;
    private Coroutine previewCoroutine;

    private Spec spec;


    private float orderTargetPosY;
    private bool isOrderAnimating;

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
        basePosX = rectTransform.anchoredPosition.x;

        orderTargetPosY = 0;
        isOrderAnimating = false;

        previewCoroutine = null;
        previewState = CardPreviewStates.IDLE;
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
        if (previewState != CardPreviewStates.IDLE && previewState != CardPreviewStates.PREVIEW_TO_IDLE)
        {
            if (previewCoroutine != null)
            {
                StopCoroutine(previewCoroutine);
            }

            float currentPosX = rectTransform.anchoredPosition.x;
            previewCoroutine = StartCoroutine(Translate(new Vector2(currentPosX, basePosX), CardPreviewStates.PREVIEW_TO_IDLE, CardPreviewStates.IDLE));
        }

        // backgroundImage.color = Color.white;
        // cardAnimator.ResetTrigger("PREVIEW");
        // cardAnimator.SetTrigger("NORMAL");
    }

    public void SetAsPreview()
    {
        // backgroundImage.color = Color.yellow;
        // cardAnimator.ResetTrigger("NORMAL");
        // cardAnimator.SetTrigger("PREVIEW");

        if (previewState != CardPreviewStates.PREVIEW && previewState != CardPreviewStates.IDLE_TO_PREVIEW)
        {
            if (previewCoroutine != null)
            {
                StopCoroutine(previewCoroutine);
            }

            float currentPosX = rectTransform.anchoredPosition.x;
            previewCoroutine = StartCoroutine(Translate(new Vector2(currentPosX, basePosX - previewOffsetX), CardPreviewStates.IDLE_TO_PREVIEW, CardPreviewStates.PREVIEW));
        }
    }

    public void UpdateOrderPosY()
    {
        float deltaY = orderTargetPosY - rectTransform.anchoredPosition.y;

        if (Math.Abs(deltaY) > 0.5f)
        {
            float newPosY = rectTransform.anchoredPosition.y + deltaY * 0.01f;
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newPosY);
        }
    }

    public IEnumerator Translate(Vector2 interval, CardPreviewStates transitionState, CardPreviewStates endState)
    {
        previewState = transitionState;

        int frameCount = 20;

        for (int i = 0; i < frameCount; i++)
        {
            float frameRatio = i / (frameCount * 1f);
            float newPosX = Mathf.Lerp(interval.x, interval.y, frameRatio);
            rectTransform.anchoredPosition = new Vector2(newPosX, rectTransform.anchoredPosition.y);

            yield return new WaitForFixedUpdate();
        }

        previewState = endState;
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
    public float OrderTargetPosY { set => orderTargetPosY = value; }
}