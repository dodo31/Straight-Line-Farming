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

        basePosX = transform.position.x;
    }

    public void SetAsNormal()
    {
        if (previewState != CardPreviewStates.IDLE && previewState != CardPreviewStates.PREVIEW_TO_IDLE)
        {
            if (previewCoroutine != null)
            {
                StopCoroutine(previewCoroutine);
            }

            previewCoroutine = StartCoroutine(Translate(new Vector2(transform.position.x, basePosX), CardPreviewStates.PREVIEW_TO_IDLE, CardPreviewStates.IDLE));
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

            previewCoroutine = StartCoroutine(Translate(new Vector2(transform.position.x, basePosX + previewOffsetX), CardPreviewStates.IDLE_TO_PREVIEW, CardPreviewStates.PREVIEW));
        }
    }

    // public void UpdateOrderPosY()
    // {
    //     float deltaY = orderTargetPosY - transform.position.y;
    //     float deltaSign = Math.Sign(deltaY);
    //     float distance = Math.Abs(deltaY);
        
    //     if (distance > 0.5f)
    //     {
    //         transform.position = new Vector2(transform.position.x, transform.position.y + (deltaSign * distance) * 0.5f);
    //     }
    // }

    public IEnumerator Translate(Vector2 interval, CardPreviewStates transitionState, CardPreviewStates endState)
    {
        previewState = transitionState;

        int frameCount = 20;

        for (int i = 0; i < frameCount; i++)
        {
            float frameRatio = i / (frameCount * 1f);
            float newPosX = Mathf.Lerp(interval.x, interval.y, frameRatio);
            transform.position = new Vector2(newPosX, transform.position.y);

            yield return new WaitForFixedUpdate();
        }

        previewState = endState;
    }

    public void Validate()
    {
        Debug.Log($"Panel was validated! {spec.ClientName} is happy!");
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