
using System.Threading.Tasks;
using UnityEngine;

public class FarmTileController : TileController
{
    private bool isHovered;

    protected new void Awake()
    {
        base.Awake();

        isHovered = false;

        SetInactive();
    }

    public void SowPlant(PlantController plant, PlantTypes plantType, Sprite plantSprite)
    {
        DestroyExistingPlant();

        plant.transform.SetParent(transform, false);
        plant.SetPlantType(plantType);
        plant.SetPlantSprite(plantSprite);
    }

    public void CollectPlant()
    {
        DestroyExistingPlant();
    }

    private void DestroyExistingPlant()
    {
        PlantController currentPlant = this.GetComponentInChildren<PlantController>();

        if (currentPlant != null)
        {
            DestroyImmediate(currentPlant.gameObject);
        }
    }

    public void SetHovered()
    {
        SetOverlayColor(new Color(1, 1, 1, 0.25f));
    }

    public void SetActive()
    {
        SetOverlayColor(new Color(1, 1, 1, 0.5f));
    }

    public void SetInactive()
    {
        SetOverlayColor(new Color(1, 1, 1, 0));
    }
}