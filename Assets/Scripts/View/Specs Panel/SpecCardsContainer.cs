using UnityEngine;

public class SpecCardsContainer : MonoBehaviour
{
    private const float PADDING_TOP = SpecCard.IDLE_POS_X_MARGIN;
    private const float CARD_SPACING = 5;
    
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
        foreach (SpecCard specCard in GetSpecCards())
        {
            specCard.TargetPosY = CreateCardPosY(specCard);

            if (specCard.HasLeaved)
            {
                DestroyImmediate(specCard.gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        foreach (SpecCard specCard in GetSpecCards())
        {
            specCard.UpdatePosX();
            specCard.UpdatePosY();
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

        float cardPosX = cardSize.x / 2 + SpecCard.HIDDEN_POS_X_MARGIN;
        float cardPosY = CreateCardPosY(specCard);

        cardTransform.anchoredPosition = new Vector2(cardPosX, cardPosY);

        specCard.OrderTargetPosX = -cardSize.x / 2 + SpecCard.IDLE_POS_X_MARGIN;

        foreach (PlantCount plantCount in spec.RequiredPlantCounts)
        {
            AddCountIndicatorToPanel(specCard, plantCount.Type, plantCount.Count);
        }
    }

    private float CreateCardPosY(SpecCard specCard)
    {
        RectTransform cardTransform = ((RectTransform)specCard.transform);
        Vector2 cardSize = cardTransform.sizeDelta;

        int cardIndex = specCard.transform.GetSiblingIndex();

        if (cardIndex == 0)
        {
            return rectTransform.anchoredPosition.y + cardSize.y / 2 - PADDING_TOP;
        }
        else
        {
            RectTransform previousCardTransform = (RectTransform)transform.GetChild(cardIndex - 1);
            return previousCardTransform.anchoredPosition.y - previousCardTransform.sizeDelta.y - CARD_SPACING;
        }
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