
using UnityEngine;

public class FarmTileController : TileController
{
    protected new void Awake()
    {
        base.Awake();
    }

    public void SowPlant(PlantController plant, PlantTypes plantType, Sprite plantSprite)
    {
        PlantController currentPlant = GetCurrentPlant();

        if (currentPlant == null)
        {
            plant.transform.SetParent(transform, false);
            plant.SetPlantType(plantType);
            plant.SetPlantSprite(plantSprite);
            plant.GetComponent<SpriteRenderer>().sortingOrder = this.GetComponentInChildren<SpriteRenderer>().sortingOrder + 50;
        }
    }

    public void CollectPlant()
    {
        DestroyExistingPlant();
    }

    private void DestroyExistingPlant()
    {
        PlantController currentPlant = GetCurrentPlant();

        if (currentPlant != null)
        {
            currentPlant.EjectAndDestroy();
        }

        currentPlant = null;
    }

    public PlantController GetCurrentPlant()
    {
        return GetComponentInChildren<PlantController>();
    }
}