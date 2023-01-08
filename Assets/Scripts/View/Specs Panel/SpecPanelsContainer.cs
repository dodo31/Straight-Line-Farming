using UnityEngine;

public class SpecPanelsContainer : MonoBehaviour
{
    [SerializeField]
    private PlantsDescription plantsDescription;

    [SerializeField]
    private SpecPanel specPanelPrefab;

    [SerializeField]
    private PlantCountIndicator plantCountIndicatorPrefab;

    public SpecPanel AddSpecPanel(Spec spec)
    {
        SpecPanel specPanel = Instantiate(specPanelPrefab);
        specPanel.transform.SetParent(transform);
        specPanel.SetSpec(spec);
        return specPanel;
    }

    public void AddCountIndicatorToPanel(SpecPanel panel, PlantTypes plantType, int plantCount)
    {
        PlantDescription plantDescription = plantsDescription.GetDescription(plantType);
        PlantCountIndicator plantCounIndicator = Instantiate(plantCountIndicatorPrefab);
        panel.AddRequiredPlantCount(plantDescription, plantCounIndicator, plantCount);
    }
    public SpecPanel[] GetSpecPanels()
    {
        return GetComponentsInChildren<SpecPanel>();
    }

}