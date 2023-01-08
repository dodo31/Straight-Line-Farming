using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private PlantController plantPrefab;

    [SerializeField]
    private TileController borderTilePrefab;

    [SerializeField]
    private TileController farmTilePrefab;

    [SerializeField]
    private PlantsDescription plantsDescription;

    [SerializeField]
    private Transform tilesContainer;

    [SerializeField]
    private LineSelectionController lineSelection;

    [SerializeField]
    private TruckController truck;

    [SerializeField]
    private GridSizes gridSizes;

    private Grid grid;

    private GridStates gridState;

    private bool isDraggingFromTile;
    private TileController dragStartTile;

    protected void Awake()
    {
        grid = new Grid();

        gridState = GridStates.IDLE;

        isDraggingFromTile = false;
        dragStartTile = null;
    }

    protected void Start()
    {
        truck.OnTravelUpdated += Handle_OnTruckTravelUpdated;
        truck.OnTravelCompleted += Handle_OnTruckTravelCompleted;

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        float totalWidth = 0;
        float totalHeight = 0;

        for (int columnIndex = 0; columnIndex < grid.Tiles.Count; columnIndex++)
        {
            List<Tile> column = grid.Tiles[columnIndex];

            for (int rowIndex = 0; rowIndex < grid.Tiles.Count; rowIndex++)
            {
                Tile tile = column[rowIndex];
                TileController newTileController = null;

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
        switch (gridState)
        {
            case GridStates.IDLE:
                ManageRowSelection();
                break;
        }
    }

    private void ManageRowSelection()
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
            TileController startTruckTile = null;
            TileController endTruckTile = null;

            if (TrySelectTile(out TileController endTile))
            {
                Vector2Int startCoord = dragStartTile.Tile.Coord;
                Vector2Int endCoord = endTile.Tile.Coord;
                List<Vector2Int> truckPath = GetTotalTruckPath(startCoord, endCoord, false);

                if (truckPath.Count > 0)
                {
                    startTruckTile = GetTileController(grid.GetTile(truckPath[0]));
                    endTruckTile = GetTileController(grid.GetTile(truckPath[^1]));
                    lineSelection.UpdateRowLine(startTruckTile.transform.position, endTruckTile.transform.position);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                lineSelection.EndSelection();

                if (startTruckTile != null && endTruckTile != null)
                {
                    truck.SowRow(startTruckTile.transform.position, endTruckTile.transform.position);
                    gridState = GridStates.SOWING;
                }

                isDraggingFromTile = false;
            }
            else
            {
                lineSelection.UpdateSelectionLine();
            }
        }
    }

    private List<Vector2Int> GetTotalTruckPath(Vector2Int startCoord, Vector2Int endCoord, bool onlyOneDirection = false)
    {
        Directions selectionDirection = GridUtils.CoordDeltaToDirection(startCoord, endCoord);
        Tile currentTile = dragStartTile.Tile;

        if (currentTile.Type == TileTypes.Empty || startCoord == endCoord)
        {
            return new List<Vector2Int>();
        }

        List<Vector2Int> truckPath = new List<Vector2Int>();

        int safeCount = 0;
        if (!onlyOneDirection && currentTile.Type != TileTypes.Border)
        {
            Directions oppositeDirection = GridUtils.GetOppositeDirection(selectionDirection);
            do
            {
                Vector2Int currentCoord = GridUtils.CoordDirectionToCoordDelta(currentTile.Coord, oppositeDirection);
                currentTile = grid.GetTile(currentCoord);
                safeCount++;
            } while (currentTile != null && currentTile.Type == TileTypes.Farm && safeCount < 30);

            if (currentTile == null)
            {
                return new List<Vector2Int>();
            }

            safeCount = 0;
        }

        bool wentOnFarm = currentTile.Type == TileTypes.Farm;
        do
        {
            Vector2Int currentCoord = GridUtils.CoordDirectionToCoordDelta(currentTile.Coord, selectionDirection);
            currentTile = grid.GetTile(currentCoord);

            if (currentTile.Type == TileTypes.Farm)
            {
                truckPath.Add(currentCoord);
            }

            wentOnFarm |= currentTile.Type == TileTypes.Farm;

            safeCount++;
        } while (currentTile != null && currentTile.Type == TileTypes.Farm && safeCount < 30);

        return truckPath;
    }

    private void SowPlant(PlantTypes plantType, FarmTileController targetTile)
    {
        PlantDescription plantDescription = plantsDescription.GetDescription(plantType);

        PlantController newPlant = Instantiate(plantPrefab);
        targetTile.SowPlant(newPlant, plantType, plantDescription.Sprite);
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

    private void Handle_OnTruckTravelUpdated()
    {
        switch (gridState)
        {
            case GridStates.SOWING:
                break;
        }
    }
    
    private void Handle_OnTruckTravelCompleted()
    {
        Debug.Log("OK");
        gridState = GridStates.IDLE;
    }
}