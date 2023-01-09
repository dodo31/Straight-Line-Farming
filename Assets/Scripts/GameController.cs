using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GridController gridController;

    [SerializeField]
    private SpecsController specsController;

    [SerializeField]
    private EconomyController economyController;

    [SerializeField]
    private ActionPanel actionPanel;

    [SerializeField]
    private CompostArea compostArea;

    [SerializeField]
    private PlantsDescription plantsDescription;

    [SerializeField]
    private GraphicRaycaster graphicRaycaster;
    
    private UserAction selectedAction;

    private bool isDraggingFromTile;

    protected void Awake()
    {
        selectedAction = null;
        isDraggingFromTile = false;

        gridController.OnTruckOverTile += Handle_OnTruckOverTile;
        gridController.OnTruckTravelCompleted += Handle_OnTruckTravelCompleted;

        compostArea.OnWasteReceivingComplete += Handle_OnWasteReceivingComplete;
    }

    protected void Start()
    {
        selectedAction = actionPanel.GetSelectedAction();
        
        economyController.GainMoney(1000);
    }
    protected void Update()
    {
        if (!isDraggingFromTile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gridController.IsIdle && gridController.TrySelectTile(graphicRaycaster, out TileController startTile))
                {
                    gridController.StartRowSelection(startTile);
                    isDraggingFromTile = true;
                }
            }
        }
        else
        {
            selectedAction = actionPanel.GetSelectedAction();
            bool hasSelectionChanged = gridController.UpdateRowSelection(graphicRaycaster, selectedAction);

            if (selectedAction.ActionType == UserActionTypes.COLLECT)
            {
                if (hasSelectionChanged)
                {
					SpecCard[] specCards = SpecCardsToValidate(gridController.CurrentPathPlants, out PlantCount[] plantsGarbage);

                    foreach (SpecCard specCard in specsController.GetSpecCards())
                    {
                        if (specCards.Contains(specCard))
                        {
                            specCard.SetAsPreview();
                        }
                        else
                        {
                            specCard.SetAsNormal();
                        }
                    }
					compostArea.Refresh(plantsGarbage);
				}
			} else {
				if(hasSelectionChanged)
				{
					foreach (SpecCard specCard in specsController.GetSpecCards())
					{
						specCard.SetAsNormal();
					}
				}
			}

            if (Input.GetMouseButtonUp(0))
            {
                if (gridController.currentTileLine.Count >= 2)
                {
                    economyController.UseMoney(100);
                    specsController.DecreaseDeadlines();
                }
                gridController.EndRowSelection(selectedAction);
                isDraggingFromTile = false;
            }
        }
    }

    private void Handle_OnTruckOverTile(FarmTileController tile)
    {
        switch (gridController.GridState)
        {
            case GridStates.FARMING:
                if (selectedAction is UserSowAction sowAction)
                {
                    PlantDescription plantDescription = plantsDescription.GetDescription(sowAction.PlantType);

                    economyController.UseMoney(plantDescription.price);
                    gridController.SowPlant(sowAction.PlantType, plantDescription.GridSprite, tile);
                }
                else if (selectedAction is UserCollectAction collectAction)
                {
                    gridController.CollectPlant(tile);
                }
                break;
        }
    }

    private void Handle_OnTruckTravelCompleted(List<Vector2Int> truckPath)
    {
        UserAction action = actionPanel.GetSelectedAction();
        if (action.ActionType == UserActionTypes.COLLECT)
        {
            SpecCard[] specCards = SpecCardsToValidate(gridController.CurrentPathPlants, out _);

            foreach (SpecCard specCard in specCards)
            {
                economyController.GainMoney(specCard.Spec.Gain);
                specCard.Validate();
            }
        }
        
        compostArea.AcceptWastes();
    }

    public SpecCard[] SpecCardsToValidate(List<Vector2Int> truckPath, out PlantCount[] garbage)
    {
        PlantCount[] plantCounts = gridController.PlantCountsFromPath(truckPath);
        return SpecCardsToValidate(plantCounts, out garbage);
    }

    public SpecCard[] SpecCardsToValidate(PlantCount[] plantCounts, out PlantCount[] garbage)
    {
        List<SpecCard> cardsToValidate = new();
        SpecCard[] specCards = specsController.GetSpecCards();

        garbage = plantCounts;

        foreach (SpecCard specCard in specCards)
        {
            PlantCount[] cardPlantCount = specCard.Spec.RequiredPlantCounts;
            bool enough = GridController.IsPlantCountArrayEnough(plantCounts, cardPlantCount, out PlantCount[] remainder);

            if (enough)
            {
                cardsToValidate.Add(specCard);
                plantCounts = remainder;
                garbage = plantCounts;
            }
        }

        return cardsToValidate.ToArray();
    }

    private void Handle_OnWasteReceivingComplete()
    {

    }
}