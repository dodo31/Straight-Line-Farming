using System.Collections.Generic;

public class Spec
{
    private string clientName;
    private string clientSpritePath;

    private List<PlantCount> requiredPlantCounts;

    private int deadline;
    private int gain;

    public Spec(string clientName, string clientSpritePath, int deadline, int gain)
    {
        this.clientName = clientName;
        this.clientSpritePath = clientSpritePath;

        requiredPlantCounts = new List<PlantCount>();

        this.deadline = deadline;
        this.gain = gain;
    }

    public void AddPlantCount(PlantCount plantCount)
    {
        requiredPlantCounts.Add(plantCount);
    }

    public string ClientName { get => clientName; }
    public string ClientSpritePath { get => clientSpritePath; }

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