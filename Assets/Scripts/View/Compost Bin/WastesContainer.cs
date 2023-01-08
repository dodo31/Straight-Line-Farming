using UnityEngine;

public class WastesContainer : MonoBehaviour
{

    [SerializeField]
    private PlantWaste plantWastePrefab1;

    [SerializeField]
    private PlantWaste plantWastePrefab2;

    [SerializeField]
    private PlantWaste plantWastePrefab3;

    [SerializeField]
    private PlantWaste plantWastePrefab4;


    [SerializeField]
    private PlantsDescription plantsDescription;


    [SerializeField]
    private Transform wasteAnchor1;

    [SerializeField]
    private Transform wasteAnchor2;

    [SerializeField]
    private Transform wasteAnchor3;

    [SerializeField]
    private Transform wasteAnchor4;

    public void AddWaste(int slotIndex, PlantTypes plantType, int wasteCount)
    {
        switch (slotIndex)
        {
            case 0:
                AppearWaste(plantType, wasteCount, wasteAnchor1, plantWastePrefab1);
                break;
            case 1:
                AppearWaste(plantType, wasteCount, wasteAnchor2, plantWastePrefab2);
                break;
            case 2:
                AppearWaste(plantType, wasteCount, wasteAnchor3, plantWastePrefab3);
                break;
            case 3:
                AppearWaste(plantType, wasteCount, wasteAnchor4, plantWastePrefab4);
                break;
        }
    }

    private void AppearWaste(PlantTypes plantType, int wasteCount, Transform wasteAnchor, PlantWaste wastPrefab)
    {
        PlantDescription plantDescription = plantsDescription.GetDescription(plantType);

        PlantWaste newWaste = Instantiate(wastPrefab);
        newWaste.transform.SetParent(wasteAnchor.transform, false);

        newWaste.SetPlantSprite(plantDescription.UiSprite);
        newWaste.SetPlantCount(wasteCount);
    }

    public void RejectWastes()
    {
        PlantWaste[] plantWastes = GetWastes();

        foreach (PlantWaste plantWaste in plantWastes)
        {
            plantWaste.Disappear();
        }
    }

    public void AcceptWastes()
    {
        PlantWaste[] plantWastes = GetWastes();

        foreach (PlantWaste plantWaste in plantWastes)
        {
            plantWaste.Throw();
        }
    }

    private PlantWaste[] GetWastes()
    {
        return GetComponentsInChildren<PlantWaste>();
    }
}
