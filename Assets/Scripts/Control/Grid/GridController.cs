using System;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private TileController borderTilePrefab;

    [SerializeField]
    private TileController farmTilePrefab;

    [SerializeField]
    private Transform tilesContainer;

    [SerializeField]
    private LineSelectionController lineSelection;

    private Grid grid;

    private bool isDraggingTile;

    protected void Start()
    {
        grid = new Grid();

        isDraggingTile = false;

        GenerateGrid();
    }

    protected void Update()
    {
        if (!isDraggingTile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);

                if (hit.collider != null && hit.transform.TryGetComponent<TileController>(out TileController tileController))
                {
                    lineSelection.StartSelection();
                    isDraggingTile = true;
                    
                    Debug.Log(isDraggingTile);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                lineSelection.EndSelection();
                isDraggingTile = false;
            }
            else
            {
                lineSelection.UpdateSelection();
            }
        }
    }

    private void GenerateGrid()
    {
        float totalWidth = 0;
        float totalHeight = 0;

        Vector2 centerCoord = new Vector2Int(grid.RowCount / 2, grid.ColumnCount / 2) + new Vector2(0.5f, 0.5f);

        for (int columnIndex = 0; columnIndex < grid.Tiles.Count; columnIndex++)
        {
            List<Tile> column = grid.Tiles[columnIndex];

            for (int rowIndex = 0; rowIndex < grid.Tiles.Count; rowIndex++)
            {
                Tile tile = column[rowIndex];
                TileController newTileController = null;

                float centerDistance = Vector2.Distance(centerCoord, tile.Coord + new Vector2(0.5f, 0.25f));

                if (centerDistance <= 2.5f)
                {
                    newTileController = Instantiate(farmTilePrefab);
                }
                else
                {
                    newTileController = Instantiate(borderTilePrefab);
                }

                newTileController.transform.SetParent(tilesContainer);

                Vector2 tilePosition = GridUtils.GetScreenPosFromGridPos(tile.Coord);
                newTileController.transform.localPosition = tilePosition;

                newTileController.SetTile(tile);

                if (tilePosition.x + GridUtils.TILES_WIDTH > totalWidth)
                {
                    totalWidth = tilePosition.x + GridUtils.TILES_WIDTH;
                }

                if (tilePosition.y + GridUtils.TILES_HEIGHT > totalHeight)
                {
                    totalHeight = tilePosition.y + GridUtils.TILES_HEIGHT;
                }
            }
        }

        tilesContainer.position = tilesContainer.position - new Vector3(totalWidth, totalHeight, 0) * 0.5f;
    }

    private TileController[] GetTiles()
    {
        return GetComponentsInChildren<TileController>();
    }
}