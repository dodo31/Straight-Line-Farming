using System;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private GridSizes gridSizes;

    private Grid grid;

    private bool isDraggingFromTile;
    private TileController dragStartTile;

    protected void Start()
    {
        grid = new Grid();

        isDraggingFromTile = false;
        dragStartTile = null;

        GenerateGrid();
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

                //float centerDistance = Vector2.Distance(centerCoord, tile.Coord + new Vector2(0.5f, 0.25f));

                if (gridSizes.gridSizes[rowIndex][columnIndex] == 'B')
                {
                    newTileController = Instantiate(farmTilePrefab);
                    tile.Type = TileTypes.Farm;
                }
                else
                {
                    newTileController = Instantiate(borderTilePrefab);
                    tile.Type = TileTypes.Border;
                }

                newTileController.transform.SetParent(tilesContainer);

                Vector2 tilePosition = GridUtils.CoordToScreenPosition(tile.Coord);
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

    protected void Update()
    {
        if (!isDraggingFromTile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (TrySelectTile(out TileController startTile))
                {
                    lineSelection.StartSelection();
                    dragStartTile = startTile;
                    isDraggingFromTile = true;
                }
            }
        }
        else
        {
            if (TrySelectTile(out TileController endTile))
            {
                RefreshRowSelection(endTile);
            }

            if (Input.GetMouseButtonUp(0))
            {
                lineSelection.EndSelection();
                isDraggingFromTile = false;
            }
            else
            {
                lineSelection.UpdateSelectionLine();
            }
        }
    }

    private void RefreshRowSelection(TileController endTile)
    {
        Vector2Int startCoord = dragStartTile.Tile.Coord;
        Vector2Int endCoord = endTile.Tile.Coord;
        Directions selectionDirection = GridUtils.CoordDeltaToDirection(startCoord, endCoord);
        Tile currentTile = dragStartTile.Tile;

        if (currentTile.Type != TileTypes.Empty)
        {


            List<Vector2Int> coords = new List<Vector2Int>();

            int safeCount = 0;

            //Debug.Log("======================");

            Debug.Log("FROM: " + startCoord);
            Debug.Log("TO: " + endCoord);
            Debug.Log("DIRECTION : " + selectionDirection);
            bool wentOnFarm = currentTile.Type == TileTypes.Farm;
            do
            {
                //Debug.Log(currentTile.Type);
                Vector2Int currentCoord = GridUtils.CoordDirectionToCoordDelta(currentTile.Coord, selectionDirection);
                currentTile = grid.GetTile(currentCoord);

                coords.Add(currentCoord);
                //Debug.Log(currentCoord);
                wentOnFarm |= currentTile.Type == TileTypes.Farm;

                safeCount++;
            } while (currentTile != null && currentTile.Type == TileTypes.Farm && safeCount < 30);




            TileController realdEndTile = GetTileController(currentTile);

            if (realdEndTile && wentOnFarm)
            {
                lineSelection.UpdateRowLine(dragStartTile.transform.position, realdEndTile.transform.position);
            } else
            {
                lineSelection.EndSelection();
            }
        }
    }

    private bool TrySelectTile(out TileController tile)
    {
        Vector2 selectedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return TrySelectTile(selectedPosition, out tile);
    }

    private bool TrySelectTile(Vector2 position, out TileController tile)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, -Vector2.up);

        if (hit.collider != null)
        {
            return hit.transform.TryGetComponent<TileController>(out tile);
        }
        else
        {
            tile = null;
            return false;
        }
    }

    private TileController GetTileController(Tile tile)
    {
        TileController[] tileControllers = GetTileControllers();
        return tileControllers.FirstOrDefault(tileController => tileController.Tile == tile);
    }

    private TileController[] GetTileControllers()
    {
        return GetComponentsInChildren<TileController>();
    }
}