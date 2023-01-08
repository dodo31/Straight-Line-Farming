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

    private Spec spec;

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
        backgroundImage.color = Color.white;
    }

    public void SetAsPreview()
    {
        backgroundImage.color = Color.yellow;
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
}