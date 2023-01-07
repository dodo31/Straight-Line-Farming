using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private TileController BorderTilePrefab;

    [SerializeField]
    private TileController FarmTilePrefab;

    private Grid grid;

    protected void Start()
    {
        grid = new Grid();
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        foreach (Transform tileTransform in GetComponentInChildren<Transform>())
        {
            DestroyImmediate(tileTransform.gameObject);
        }

        Vector2 centerCoord = new Vector2Int(grid.RowCount / 2, grid.ColumnCount / 2) + new Vector2(0.5f, 0.5f);

        for (int columnIndex = 0; columnIndex < grid.Tiles.Count; columnIndex++)
        {
            List<Tile> column = grid.Tiles[columnIndex];

            for (int rowIndex = 0; rowIndex < grid.Tiles.Count; rowIndex++)
            {
                Tile tile = column[rowIndex];
                TileController newTile = null;

                float centerDistance = Vector2.Distance(centerCoord, tile.Coord + new Vector2(0.5f, 0.25f));

                if (centerDistance <= 2.5f)
                {
                    newTile = Instantiate(FarmTilePrefab);
                }
                else
                {
                    newTile = Instantiate(BorderTilePrefab);
                }

                newTile.transform.SetParent(transform);
                newTile.transform.localPosition = GridUtils.GetScreenPosFromGridPos(tile.Coord);
            }
        }
    }

    private TileController[] GetTiles()
    {
        return GetComponentsInChildren<TileController>();
    }
}