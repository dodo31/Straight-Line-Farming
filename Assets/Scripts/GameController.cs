using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GridController gridController;

    [SerializeField]
    private ActionPanel actionPanel;

    [SerializeField]
    private CompostArea compostArea;

    [SerializeField]
    private GraphicRaycaster graphicRaycaster;
    
    private bool isDraggingFromTile;

    protected void Awake()
    {
        isDraggingFromTile = false;

        gridController.OnTruckOverTile += Handle_OnTruckOverTile;
        compostArea.OnWasteReceivingComplete += Handle_OnWasteReceivingComplete;
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
            UserAction selectedAction = actionPanel.GetSelectedAction();

            gridController.UpdateRowSelection(graphicRaycaster, selectedAction);

            if (Input.GetMouseButtonUp(0))
            {
                gridController.EndRowSelection(selectedAction);
                isDraggingFromTile = false;
            }
        }
    }

    private void Handle_OnTruckOverTile(FarmTileController tile)
    {
        UserAction selectedAction = actionPanel.GetSelectedAction();

        switch (gridController.GridState)
        {
            case GridStates.FARMING:
                if (selectedAction is UserSowAction sowAction)
                {
                    gridController.SowPlant(sowAction.PlantType, tile);
                }
                else if (selectedAction is UserCollectAction collectAction)
                {
                    gridController.CollectPlant(tile);
                }
                break;
        }
    }

    private void Handle_OnWasteReceivingComplete()
    {
        Debug.Log("OK");
    }
}