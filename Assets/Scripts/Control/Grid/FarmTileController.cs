
using UnityEngine;

public class FarmTileController : TileController
{
    protected new void Awake()
    {
        base.Awake();
        SetInactive();
    }

    private void OnMouseEnter()
    {
        Debug.Log("OK");
        SetHovered();
    }

    private void OnMouseExit()
    {
        SetInactive();
    }

    public void SetHovered()
    {
        SetOverlayColor(new Color(1, 1, 1, 0.25f));
    }

    public void SetActive()
    {
        SetOverlayColor(new Color(1, 1, 1, 0.5f));
    }

    public void SetInactive()
    {
        SetOverlayColor(new Color(1, 1, 1, 0));
    }
}