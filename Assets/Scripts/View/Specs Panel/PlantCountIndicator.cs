using UnityEngine;
using UnityEngine.UI;

public class PlantCounIndicator : MonoBehaviour
{
    [SerializeField]
    private Image plantImage;

    [SerializeField]
    private PlantTypes type;

    [SerializeField]
    private Text countText;

    public void SetPlantIcon(Sprite plantIcon)
    {
        plantImage.sprite = plantIcon;
    }

    public void SetPlantType(PlantTypes plantType)
    {
        type = plantType;
    }

    public void SetPlantCount(int plantCount)
    {
        countText.text = plantCount.ToString();
    }
}