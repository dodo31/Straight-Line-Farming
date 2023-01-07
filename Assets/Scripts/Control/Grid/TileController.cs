using UnityEngine;

public abstract class TileController : MonoBehaviour
{
    protected Tile tile;

    [SerializeField]
    protected SpriteRenderer overlayRenderer;

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