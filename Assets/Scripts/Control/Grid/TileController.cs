using System;
using UnityEngine;

public abstract class TileController : MonoBehaviour
{
    protected Tile tile;

    private bool isSelected;

    [SerializeField]
    protected SpriteRenderer overlayRenderer;

    public event Action<TileController, Vector2> OnDragStart;
    public event Action<TileController> OnDragUpdate;
    public event Action<TileController> OnDragEnd;

    protected void Awake()
    {
        tile = null;

        isSelected = false;

        SetInactive();
    }

    public void SetTile(Tile tile)
    {
        this.tile = tile;
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

    public void Select()
    {
        SetOverlayColor(new Color(0.9294118f, 0.7176471f, 0.03921569f, 0.5f));
        isSelected = true;
    }

    public void Unselect()
    {
        SetInactive();
        isSelected = false;
    }

    private void SetInactive()
    {
        SetOverlayColor(new Color(1, 1, 1, 0));
    }

    protected void SetOverlayColor(Color color)
    {
        overlayRenderer.color = color;
    }

    public Tile Tile { get => tile; }
}