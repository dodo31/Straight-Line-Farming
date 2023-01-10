using UnityEngine;

public class LineSelectionController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer userLine;

    [SerializeField]
    private LineRenderer rowLine;

    private Vector2 screenStartPosition;
    private bool useTilePos = true;

    protected void Awake()
    {
        screenStartPosition = Vector2.zero;
    }

    public void StartSelection(Vector2 startPosition)
    {
        if(useTilePos)screenStartPosition = startPosition;
        else        screenStartPosition = Input.mousePosition;

        userLine.enabled = true;
    }

    public void UpdateSelectionLine()
    {
        Vector2 screenEndPosition = Input.mousePosition;

        Vector2 startPositionCamera = useTilePos? screenStartPosition:Camera.main.ScreenToWorldPoint(screenStartPosition);
        Vector2 endPositionCamera = Camera.main.ScreenToWorldPoint(screenEndPosition);

        userLine.SetPosition(0, startPositionCamera);
        userLine.SetPosition(1, endPositionCamera);
    }

    public void UpdateRowLine(Vector2 startTilePosition, Vector2 endTilePosition)
    {
        rowLine.enabled = !startTilePosition.Equals(endTilePosition);
        rowLine.SetPosition(0, startTilePosition);
        rowLine.SetPosition(1, endTilePosition);
    }

    public void EndSelection()
    {
        userLine.enabled = false;
        rowLine.enabled = false;
    }
}
