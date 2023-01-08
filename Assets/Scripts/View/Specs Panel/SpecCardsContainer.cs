using UnityEngine;

public class SpecCardsContainer : MonoBehaviour
{
    [SerializeField]
    private PlantsDescription plantsDescription;

    [SerializeField]
    private SpecCard specCardPrefab;

    [SerializeField]
    private PlantCountIndicator plantCountIndicatorPrefab;

    public SpecCard AddSpecPanel(Spec spec)
    {
        SpecCard specPanel = Instantiate(specCardPrefab);
        specPanel.transform.SetParent(transform);
        specPanel.SetSpec(spec);
        return specPanel;
    }

    public void AddCountIndicatorToPanel(SpecCard panel, PlantTypes plantType, int plantCount)
    {
        PlantDescription plantDescription = plantsDescription.GetDescription(plantType);
        PlantCountIndicator plantCounIndicator = Instantiate(plantCountIndicatorPrefab);
        panel.AddRequiredPlantCount(plantDescription, plantCounIndicator, plantCount);
    }
    public SpecCard[] GetSpecCards()
    {
        return GetComponentsInChildren<SpecCard>();
    }

}