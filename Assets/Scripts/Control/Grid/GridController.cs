using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private GridSizes gridSizes;

    [SerializeField]
    private Transform tilesContainer;

    [SerializeField]
    private LineSelectionController lineSelection;

    [SerializeField]
    private TruckController truck;

    private Grid grid;
    private GridStates gridState;

    private TileController[] tiles;

    private TileController dragStartTile;
    private List<TileController> currentTileLine;

    public event Action<FarmTileController> OnTruckOverTile;

    protected void Awake()
    {
        grid = new Grid();
        gridState = GridStates.IDLE;

        tiles = new TileController[0];

        dragStartTile = null;
        currentTileLine = new List<TileController>();
    }

    protected void Start()
    {
        truck.OnTravelUpdated += Handle_OnTruckTravelUpdated;
        truck.OnTravelCompleted += Handle_OnTruckTravelCompleted;

        GenerateGrid();
        tiles = GetComponentsInChildren<TileController>();
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

    public void StartRowSelection(TileController startTile)
    {
        lineSelection.StartSelection();
        dragStartTile = startTile;
    }

    public void UpdateRowSelection(GraphicRaycaster raycaster, UserAction currentAction)
    {
        if (TrySelectTile(raycaster, out TileController endTile))
        {
            Vector2Int startCoord = dragStartTile.Tile.Coord;
            Vector2Int endCoord = endTile.Tile.Coord;
            List<Vector2Int> truckPath = GetTotalTruckPath(startCoord, endCoord, false);

            currentTileLine.Clear();

            if (truckPath.Count >= 2)
            {
                foreach (Vector2Int tileCoord in truckPath)
                {
                    TileController tile = GetTileController(grid.GetTile(tileCoord));
                    currentTileLine.Add(tile);
                }

                Vector2 startTilePosition = currentTileLine.First().transform.position;
                Vector2 endTilePosition = currentTileLine.Last().transform.position;
                lineSelection.UpdateRowLine(startTilePosition, endTilePosition);
            }
        }

        lineSelection.UpdateSelectionLine();
    }

    public void EndRowSelection(UserAction currentAction)
    {
        lineSelection.EndSelection();

        if (currentTileLine.Count > 0)
        {
            Vector2 startTilePosition = currentTileLine.First().transform.position;
            Vector2 endTilePosition = currentTileLine.Last().transform.position;

            truck.SowRow(startTilePosition, endTilePosition);
            gridState = GridStates.FARMING;
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

    private void RefreshLineFarming()
    {
        switch (gridState)
        {
            case GridStates.FARMING:
                Vector2 truckStartPosition = truck.CurrentStartPosition;
                FarmTileController truckedTile = TindTruckedTile(truckStartPosition);

                if (truckedTile != null)
                {
                    OnTruckOverTile?.Invoke(truckedTile);
                }
                break;
        }
    }

    private FarmTileController TindTruckedTile(Vector2 truckStartPosition)
    {
        float truckPreviousDistance = Vector2.Distance(truckStartPosition, truck.PreviousTravelPosition);
        float truckCurrentDistance = Vector2.Distance(truckStartPosition, truck.CurrentTravelPosition);

        return currentTileLine.FirstOrDefault(lineTile =>
        {
            float tileDistance = Vector2.Distance(truckStartPosition, lineTile.transform.position);
            return tileDistance >= truckPreviousDistance && tileDistance < truckCurrentDistance;
        }) as FarmTileController;
    }

    public void SowPlant(PlantTypes plantType, FarmTileController targetTile)
    {
        PlantDescription plantDescription = plantsDescription.GetDescription(plantType);

        Economy.GetInstance().UseMoney(plantDescription.price);
        PlantController newPlant = Instantiate(plantPrefab);
        targetTile.SowPlant(newPlant, plantType, plantDescription.GridSprite);
    }

    public void CollectPlant(FarmTileController targetTile)
    {
        targetTile.CollectPlant();
    }

    public bool TrySelectTile(GraphicRaycaster graphicRaycaster, out TileController tile)
    {
        Vector2 selectedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return TrySelectTile(graphicRaycaster, selectedPosition, out tile);
    }

    private bool TrySelectTile(GraphicRaycaster graphicRaycaster, Vector2 position, out TileController tile)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> hitsUi = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, hitsUi);

        RaycastHit2D hit2d = Physics2D.Raycast(position, -Vector2.up);

        if (hitsUi.Count == 0 && hit2d.collider != null)
        {
            return hit2d.transform.TryGetComponent<TileController>(out tile);
        }
        else
        {
            tile = null;
            return false;
        }
    }

    public TileController GetTileController(Tile tile)
    {
        return tiles.FirstOrDefault(tileController => tileController.Tile == tile);
    }

    private void Handle_OnTruckTravelUpdated()
    {
        RefreshLineFarming();
    }

    private void Handle_OnTruckTravelCompleted()
    {
        gridState = GridStates.IDLE;
    }

    public bool IsIdle
    {
        get
        {
            return gridState == GridStates.IDLE;
        }
    }

    public bool IsFarming
    {
        get
        {
            return gridState == GridStates.FARMING;
        }
    }

    public GridStates GridState { get => gridState; }
}