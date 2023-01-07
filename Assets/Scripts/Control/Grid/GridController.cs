using UnityEngine;

public class GridController : MonoBehaviour
{
    private Grid grid;

    public GridController()
    {
        grid = new Grid();
    }

    private TileController[] GetTiles()
    {
        return GetComponentsInChildren<TileController>();
    }
}