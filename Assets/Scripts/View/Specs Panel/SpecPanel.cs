using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecPanel : MonoBehaviour
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

    private Spec spec;

    public void SetSpec(Spec spec)
    {
        this.spec = spec;

        SetClientName(spec.ClientName);
        SetClientIcon(Resources.Load<Sprite>(spec.ClientSpritePath));
        SetClientName(spec.ClientName);
        SetClientName(spec.ClientName);
    }

    public void Validate()
    {
        Economy.GetInstance().GainMoney(spec.Gain);
        Debug.Log($"Panel was validated! {spec.ClientName} is happy!");
        Destroy(this.gameObject);
    }

    public Spec GetSpec()
    {
        return spec;
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

}