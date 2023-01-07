
using UnityEngine;

public class FarmTileController : TileController
{
    private bool isHovered;

    protected new void Awake()
    {
        base.Awake();

        isHovered = false;

        SetInactive();
    }

    private void OnMouseEnter()
    {
        SetHovered();
        isHovered = true;
    }

    private void OnMouseDown()
    {
        SetActive();
    }

    private void OnMouseUp()
    {
        if (isHovered)
        {
            SetHovered();
        }
        else
        {
            SetInactive();
        }
    }

    private void OnMouseExit()
    {
        SetInactive();
        isHovered = false;
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