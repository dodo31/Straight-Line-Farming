using System.Collections.Generic;
using UnityEngine;

public class Spec
{
    private string clientName;
    private Sprite clientSprite;

    private List<PlantCount> requiredPlantCounts;

    private int deadline;
    private int gain;

    public Spec(string clientName, Sprite sprite, int deadline, int gain)
    {
        this.clientName = clientName;
        this.clientSprite = sprite;

        requiredPlantCounts = new List<PlantCount>();

        this.deadline = deadline;
        this.gain = gain;
    }

    public void AddPlantCount(PlantCount plantCount)
    {
        requiredPlantCounts.Add(plantCount);
    }
    public void DecreaseDeadline()
    {
        deadline--;
    }

    public void ResetDeadline()
    {
        deadline = ShopVars.GetInstance().baseDays;
    }

    public string ClientName { get => clientName; }
    public Sprite ClientSprite { get => clientSprite; }

    public PlantCount[] RequiredPlantCounts
    {
        get
        {
            return requiredPlantCounts.ToArray();
        }
    }

    public int Deadline { get => deadline; }
    public int Gain { get => gain; }
}