using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantCountIndicator : MonoBehaviour
{
    [SerializeField]
    private Image plantImage;

    private PlantTypes type;

    [SerializeField]
    private TMP_Text countText;

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