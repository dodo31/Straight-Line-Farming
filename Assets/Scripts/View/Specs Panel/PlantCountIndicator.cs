using System.Collections;
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

    private void Awake()
    {
        StartCoroutine(ApplyRatio());
    }

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

    private IEnumerator ApplyRatio()
    {
        yield return new WaitForEndOfFrame();
        
        RectTransform rectTransform = (RectTransform)transform;
        Vector2 indicatorSize = rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(indicatorSize.y, indicatorSize.y);
    }
}