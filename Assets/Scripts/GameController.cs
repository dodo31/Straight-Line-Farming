using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GridController gridController;

    [SerializeField]
    private ActionPanel actionPanel;

    private bool isDraggingFromTile;

    protected void Awake()
    {
        isDraggingFromTile = false;

        gridController.OnTruckOverTile += Handle_OnTruckOverTile;
    }

    protected void Update()
    {
        if (!isDraggingFromTile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gridController.TrySelectTile(out TileController startTile))
                {
                    gridController.StartRowSelection(startTile);
                    isDraggingFromTile = true;
                }
            }
        }
        else
        {
            UserAction selectedAction = actionPanel.GetSelectedAction();

            gridController.UpdateRowSelection(selectedAction);

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
            case GridStates.SOWING:
                UserSowAction sowAction = actionPanel.GetSelectedAction() as UserSowAction;

                if (sowAction != null)
                {
                    gridController.SowPlant(sowAction.PlantType, tile);
                }
                break;
            case GridStates.COLLECTING:
                UserCollectAction collectAction = actionPanel.GetSelectedAction() as UserCollectAction;

                Debug.Log(collectAction.GetType());

                if (collectAction != null)
                {
                    gridController.ConnectPlant(tile);
                }
                break;
        }
    }
}