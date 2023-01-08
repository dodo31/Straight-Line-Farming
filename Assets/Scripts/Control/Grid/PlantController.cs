
using UnityEngine;

public class PlantController : MonoBehaviour
{
    private PlantTypes plantType;

    [SerializeField]
    private SpriteRenderer plantRenderer;

    public void SetPlantType(PlantTypes plantType)
    {
        this.plantType = plantType;
    }

    public PlantTypes GetPlantType()
    {
        return plantType;
    }

    public void SetPlantSprite(Sprite plantSprite)
    {
        plantRenderer.sprite = plantSprite;
    }
}