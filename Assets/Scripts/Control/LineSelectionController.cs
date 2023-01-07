using UnityEngine;

public class LineSelectionController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer userLine;

    [SerializeField]
    private LineRenderer rowLine;

    private Vector2 screenStartPosition;

    protected void Awake()
    {
        screenStartPosition = Vector2.zero;
    }

    public void StartSelection()
    {
        screenStartPosition = Input.mousePosition;

        userLine.enabled = true;
    }

    public void UpdateSelection()
    {
        Vector2 screenEndPosition = Input.mousePosition;

        Vector2 startPositionCamera = Camera.main.ScreenToWorldPoint(screenStartPosition);
        Vector2 endPositionCamera = Camera.main.ScreenToWorldPoint(screenEndPosition);

        userLine.SetPosition(0, startPositionCamera);
        userLine.SetPosition(1, endPositionCamera);
    }

    public void EndSelection()
    {
        userLine.enabled = false;
    }
}
