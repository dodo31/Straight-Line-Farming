using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecPanel : MonoBehaviour
{
    [SerializeField]
    private Text clientText;

    [SerializeField]
    private Image clientImage;

    [SerializeField]
    private List<PlantCounIndicator> requiredPlantIndicators;

    [SerializeField]
    private Text deadlineValueText;

    [SerializeField]
    private Text deadlineUnitText;

    [SerializeField]
    private Text gainValueText;

    [SerializeField]
    private Text gainUnitText;
    
    public void AddClientName(string clientName)
    {
        clientText.name = clientName;
    }
    
    public void AddClientIcon(Sprite clientIcon)
    {
        clientImage.sprite = clientIcon;
    }

    public void AddRequiredPlantCount(PlantCounIndicator countIndicator)
    {
        throw new Exception("TODO !");
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