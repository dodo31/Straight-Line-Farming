
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

    public void SetPlantSprite(Sprite plantSprite)
    {
        plantRenderer.sprite = plantSprite;
        
        Debug.Log(plantSprite);
    }
}