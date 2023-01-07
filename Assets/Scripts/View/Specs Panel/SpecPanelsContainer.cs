using UnityEngine;

public class SpecPanelsContainer : MonoBehaviour
{
    [SerializeField]
    private PlantsDescription plantsDescription;

    [SerializeField]
    private SpecPanel specPanelPrefab;

    [SerializeField]
    private PlantCounIndicator plantCountIndicatorPrefab;

    public SpecPanel AddSpecPanel(string clientName, Sprite clientIcon, int deadline, float gain)
    {
        SpecPanel specPanel = Instantiate(specPanelPrefab);
        specPanel.transform.SetParent(transform);
        specPanel.SetClientName(clientName);
        specPanel.SetClientIcon(clientIcon);
        specPanel.SetDeadline(deadline);
        specPanel.SetGain(gain);
        return specPanel;
    }

    public void AddCountIndicatorToPanel(SpecPanel panel, PlantTypes plantType, int plantCount)
    {
        PlantDescription plantDescription = plantsDescription.GetDescription(plantType);
        PlantCounIndicator plantCounIndicator = Instantiate(plantCountIndicatorPrefab);
        panel.AddRequiredPlantCount(plantDescription, plantCounIndicator, plantCount);
    }
}