using System;
using UnityEngine;

public abstract class TileController : MonoBehaviour
{
    protected Tile tile;

    [SerializeField]
    protected SpriteRenderer overlayRenderer;

    public event Action<TileController, Vector2> OnDragStart;
    public event Action<TileController> OnDragUpdate;
    public event Action<TileController> OnDragEnd;

    protected void Awake()
    {
        tile = null;
    }

    protected void SetOverlayColor(Color color)
    {
        overlayRenderer.color = color;
    }

    public void SetTile(Tile tile)
    {
        this.tile = tile;
    }
}