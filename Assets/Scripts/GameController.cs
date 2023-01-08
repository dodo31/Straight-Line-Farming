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
            gridController.UpdateRowSelection();

            if (Input.GetMouseButtonUp(0))
            {
                gridController.EndRowSelection();
                isDraggingFromTile = false;
            }
        }
    }
}