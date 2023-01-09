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
            if (gridController.TryHitTile(graphicRaycaster, out TileController tile))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (gridController.IsIdle)
                    {
                        gridController.StartRowSelection(tile);
                        isDraggingFromTile = true;
                    }
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
            }
            else
            {
                if (hasSelectionChanged)
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

                    economyController.UseMoney(plantDescription.price - ShopVars.GetInstance().seedPromo * 10);
                    gridController.SowPlant(sowAction.PlantType, plantDescription.GridSprite, tile);
                    if (tile.GetCurrentPlant() == null)
                        SoundPlayer.PlaySound(SoundPlayer.SoundType.SOW);
                }
                else if (selectedAction is UserCollectAction collectAction)
                {
                    gridController.CollectPlant(tile);
                    if(tile.GetCurrentPlant() != null)
                        switch (tile.GetCurrentPlant().plantType)
                        {
                            case PlantTypes.Wheat:
                                SoundPlayer.PlaySound(SoundPlayer.SoundType.WHEAT);
                                break;
                            case PlantTypes.Corn:
                                SoundPlayer.PlaySound(SoundPlayer.SoundType.CORN);
                                break;
                            case PlantTypes.Pumpkin:
                                SoundPlayer.PlaySound(SoundPlayer.SoundType.PUMPKIN);
                                break;
                            case PlantTypes.Chilli:
                                SoundPlayer.PlaySound(SoundPlayer.SoundType.CHILLI);
                                break;
                        }
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
                SoundPlayer.PlaySound(SoundPlayer.SoundType.THANKS);
                SoundPlayer.PlaySound(SoundPlayer.SoundType.GLING);
            }
        }
        specsController.DecreaseDeadlines();
        ShopVars.GetInstance().day++;
        if (economyController.GetMoney() < 0)
        {
            ShopVars.GetInstance().lives--;
            SoundPlayer.PlaySound(SoundPlayer.SoundType.ANGRY);
            economyController.SetMoney(300);
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