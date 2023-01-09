using UnityEngine;

public class SpecCardsContainer : MonoBehaviour
{
    [SerializeField]
    private PlantsDescription plantsDescription;

    [SerializeField]
    private SpecCard specCardPrefab;

    [SerializeField]
    private PlantCountIndicator plantCountIndicatorPrefab;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
    }

    public void Update()
    {
        SpecCard[] specCards = GetSpecCards();

        float curentPosY = 0;

        for (int i = 0; i < specCards.Length; i++)
        {
            SpecCard specCard = specCards[0];

            specCard.OrderTargetPosY = CreateCardPosY(specCard);
            specCard.UpdateOrderPosY();

            RectTransform cardTransform = ((RectTransform)specCard.transform);
            Vector2 cardSize = cardTransform.sizeDelta;

            curentPosY += cardSize.y + 15;
        }
    }

    public void AddSpecCard(Spec spec)
    {
        SpecCard specCard = Instantiate(specCardPrefab);
        specCard.transform.SetParent(transform);
        specCard.SetSpec(spec);

        Vector2 anchorPosition = rectTransform.anchoredPosition;

        RectTransform cardTransform = ((RectTransform)specCard.transform);
        Vector2 cardSize = cardTransform.sizeDelta;

        float cardPosX = -cardSize.x / 2;
        float cardPosY = CreateCardPosY(specCard);

        cardTransform.anchoredPosition = new Vector2(cardPosX, cardPosY);

        foreach (PlantCount plantCount in spec.RequiredPlantCounts)
        {
            AddCountIndicatorToPanel(specCard, plantCount.Type, plantCount.Count);
        }
    }

    private float CreateCardPosY(SpecCard specCard)
    {
        Vector2 anchorPosition = rectTransform.anchoredPosition;

        RectTransform cardTransform = ((RectTransform)specCard.transform);
        Vector2 cardSize = cardTransform.sizeDelta;

        return -cardSize.y / 2 - anchorPosition.y - (cardSize.y + 15) * specCard.transform.GetSiblingIndex();
    }

    private void AddCountIndicatorToPanel(SpecCard card, PlantTypes plantType, int plantCount)
    {
        PlantDescription plantDescription = plantsDescription.GetDescription(plantType);
        PlantCountIndicator plantCounIndicator = Instantiate(plantCountIndicatorPrefab);
        card.AddRequiredPlantCount(plantDescription, plantCounIndicator, plantCount);
    }

    public SpecCard[] GetSpecCards()
    {
        return GetComponentsInChildren<SpecCard>();
    }
}