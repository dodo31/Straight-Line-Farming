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
    private TileController emptyTilePrefab;

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
    public TruckController truck;

    public Grid grid;
    private GridStates gridState;

    private int oldGridSize;

    private TileController[] tiles;

    private TileController dragStartTile;
    public List<TileController> currentTileLine;

    private PlantCount[] currentPathPlants;

    public event Action<FarmTileController> OnTruckOverTile;
    public event Action<List<Vector2Int>> OnTruckTravelCompleted;

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
        PlantStartingWheat();
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
                char gridChar = gridSizes.gridSizes[rowIndex][columnIndex];
                if (gridChar == 'B' || gridChar == '-')
                {
                    newTileController = Instantiate(farmTilePrefab);
                    tile.Type = TileTypes.Farm;
                }
                else if (gridChar == 'C' || gridChar == 'D' || gridChar == 'E')
                {
                    newTileController = Instantiate(borderTilePrefab);
                    tile.Type = TileTypes.Border;
                }
                else
                {
                    newTileController = Instantiate(emptyTilePrefab);
                    tile.Type = TileTypes.Empty;
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

                var spriteRChilds = newTileController.gameObject.GetComponentsInChildren<SpriteRenderer>();

                for (int i = 0; i < spriteRChilds.Length; i++)
                {
                    SpriteRenderer spriteR = spriteRChilds[i];
                    spriteR.sortingOrder = -(int)(tilePosition.y * 10) - i;
                }
            }
        }

        tilesContainer.position = tilesContainer.position - new Vector3(totalWidth, totalHeight, 0) * 0.5f;
    }

    private void PlantStartingWheat()
    {
        for (int columnIndex = 0; columnIndex < grid.Tiles.Count; columnIndex++)
        {
            List<Tile> column = grid.Tiles[columnIndex];

            for (int rowIndex = 0; rowIndex < grid.Tiles.Count; rowIndex++)
            {
                Tile tile = column[rowIndex];

                if (gridSizes.gridSizes[rowIndex][columnIndex] == '-')
                {
                    PlantDescription plantDescription = plantsDescription.GetDescription(PlantTypes.Wheat);

                    SowPlant(PlantTypes.Wheat, plantDescription.GridSprite, (FarmTileController)GetTileController(tile));

                }

            }
        }
    }

    public void GridSizeUpdate()
    {
        currentTileLine.Clear();
        int newGridSize = ShopVars.GetInstance().gridSize;

        for (int columnIndex = 0; columnIndex < grid.Tiles.Count; columnIndex++)
        {
            List<Tile> column = grid.Tiles[columnIndex];

            for (int rowIndex = 0; rowIndex < grid.Tiles.Count; rowIndex++)
            {
                Tile tile = column[rowIndex];
                TileController newTileController = null;
                char gridChar = gridSizes.gridSizes[rowIndex][columnIndex];
                if (gridChar == 'B' + newGridSize || gridChar == 'E' + newGridSize)
                {
                    var tileCon = GetTileController(tile);

                    Destroy(tileCon.gameObject);
                    if (gridChar == 'B' + newGridSize)
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

                    var spriteRChilds = newTileController.gameObject.GetComponentsInChildren<SpriteRenderer>();

                    for (int i = 0; i < spriteRChilds.Length; i++)
                    {
                        SpriteRenderer spriteR = spriteRChilds[i];
                        spriteR.sortingOrder = -(int)(tilePosition.y * 10) - i;
                    }

                }
            }
        }
    }

    public void Update()
    {
        if (tiles.Any(tile => tile == null))
        {
            tiles = GetComponentsInChildren<TileController>();
        }
        if (ShopVars.GetInstance().gridSize > oldGridSize)
        {
            oldGridSize = ShopVars.GetInstance().gridSize;
            GridSizeUpdate();
        }
    }

    public void StartRowSelection(TileController startTile)
    {
        lineSelection.StartSelection(startTile.transform.position);
        dragStartTile = startTile;
    }

    public bool UpdateRowSelection(GraphicRaycaster raycaster, UserAction currentAction)
    {
        bool hasSelectionChanged = false;

        if (TryHitTile(raycaster, out TileController endTile))
        {
            Vector2Int startCoord = dragStartTile.Tile.Coord;
            Vector2Int endCoord = endTile.Tile.Coord;
            Directions selectionDirection = GridUtils.CoordDeltaToDirection(startCoord, endCoord);

            List<Vector2Int> truckPath = GetTotalTruckPath(startCoord, endCoord, selectionDirection, false);
            

            truck.SetDirection(selectionDirection);

            TileController[] previousTileLine = currentTileLine.ToArray();
            currentTileLine.Clear();
            

            if (GetTotalTruckPath(startCoord, endCoord, selectionDirection, true).Count >= 1 && truckPath.Count >= 2)
            {
                currentPathPlants = PlantCountsFromPath(truckPath);
                foreach (Vector2Int tileCoord in truckPath)
                {
                    TileController tile = GetTileController(grid.GetTile(tileCoord));
                    currentTileLine.Add(tile);
                }

                Vector2 startTilePosition = currentTileLine.First().transform.position;
                Vector2 endTilePosition = currentTileLine.Last().transform.position;
                lineSelection.UpdateRowLine(startTilePosition, endTilePosition);

                SelectTiles(currentTileLine);

                if (!previousTileLine.SequenceEqual(currentTileLine.ToArray()))
                {
                    hasSelectionChanged = true;
                }
            }
            else
            {
                currentPathPlants = new PlantCount[0];

                lineSelection.UpdateRowLine(new Vector2(), new Vector2());

                if (previousTileLine.Length != truckPath.Count)
                {
                    hasSelectionChanged = true;
                }

                currentTileLine.Clear();

                UnselectAllTiles();
            }
        }
        else
        {
            UnselectAllTiles();
        }

        lineSelection.UpdateSelectionLine();

        return hasSelectionChanged;
    }

    public PlantCount[] PlantCountsFromPath(List<Vector2Int> truckPath)
    {
        int[] amounts = new int[4];

        for (int i = 0; i < truckPath.Count; i++)
        {
            TileController tileController = GetTileController(grid.GetTile(truckPath[i]));
            if (tileController is FarmTileController farmTileController)
            {
                PlantController plant = farmTileController.GetCurrentPlant();

                if (plant != null)
                {
                    amounts[(int)plant.GetPlantType()]++;
                }
            }
        }

        List<PlantCount> plantCounts = new();

        for (int i = 0; i < amounts.Length; i++)
        {
            if (amounts[i] > 0)
            {
                PlantCount plantCount = new PlantCount((PlantTypes)i, amounts[i]);
                plantCounts.Add(plantCount);
            }
        }

        return plantCounts.ToArray();
    }

    public static bool IsPlantCountArrayEnough(PlantCount[] harvested, PlantCount[] spec, out PlantCount[] remainder)
    {
        List<PlantCount> remainderList = new(harvested);
        
        for (int i = 0; i < spec.Length; i++)
        {
            bool found = false;
            for (int j = 0; j < remainderList.Count && !found; j++)
            {
                if (spec[i].Type == remainderList[j].Type)
                {
                    if (spec[i].Count > remainderList[j].Count)
                    {
                        remainder = null;
                        return false;
                    }
                    else
                    {
                        found = true;
                        if (spec[i].Count < remainderList[j].Count)
                        {
                            remainderList[j] = new PlantCount(spec[i].Type, remainderList[j].Count - spec[i].Count);
                        } else if (spec[i].Count == remainderList[j].Count)
                        {
                            remainderList.RemoveAt(j);
                        }
                    }
                }
            }

            if (!found)
            {
                remainder = null;
                return false;
            }
        }

        remainder = remainderList.ToArray();
        return true;
    }

    public void EndRowSelection(UserAction currentAction)
    {
        lineSelection.EndSelection();
        UnselectAllTiles();

        if (currentTileLine.Count > 0)
        {
            Vector2 startTilePosition = currentTileLine.First().transform.position;
            Vector2 endTilePosition = currentTileLine.Last().transform.position;

            Vector2Int startCoord = currentTileLine.First().Tile.Coord;
            Vector2Int endCoord = currentTileLine.Last().Tile.Coord;
            Directions selectionDirection = GridUtils.CoordDeltaToDirection(startCoord, endCoord);
            
            List<Vector2Int> truckPath = GetTotalTruckPath(startCoord, endCoord, selectionDirection, false);

            currentPathPlants = PlantCountsFromPath(truckPath);

            truck.StartTravelRow(startTilePosition, endTilePosition);
            gridState = GridStates.FARMING;
        }
    }

    public List<Vector2Int> GetTotalTruckPath(Vector2Int startCoord, Vector2Int endCoord, Directions selectionDirection, bool onlyOneDirection = false)
    {
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
                FarmTileController truckedTile = FindTruckedTile(truckStartPosition);

                if (truckedTile != null)
                {
                    OnTruckOverTile?.Invoke(truckedTile);
                }
                break;
        }
    }

    private FarmTileController FindTruckedTile(Vector2 truckStartPosition)
    {
        float truckPreviousDistance = Vector2.Distance(truckStartPosition, truck.PreviousTravelPosition);
        float truckCurrentDistance = Vector2.Distance(truckStartPosition, truck.CurrentTravelPosition);

        return currentTileLine.FirstOrDefault(lineTile =>
        {
            float tileDistance = Vector2.Distance(truckStartPosition, lineTile.transform.position);
            return tileDistance >= truckPreviousDistance && tileDistance < truckCurrentDistance;
        }) as FarmTileController;
    }

    public void SowPlant(PlantTypes plantType, Sprite plantSprite, FarmTileController targetTile)
    {
        PlantController newPlant = Instantiate(plantPrefab);
        targetTile.SowPlant(newPlant, plantType, plantSprite);
    }

    public void CollectPlant(FarmTileController targetTile)
    {
        targetTile.CollectPlant();
    }

    public bool TryHitTile(GraphicRaycaster graphicRaycaster, out TileController tile)
    {
        Vector2 selectedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return TryHitTile(graphicRaycaster, selectedPosition, out tile);
    }

    private bool TryHitTile(GraphicRaycaster graphicRaycaster, Vector2 position, out TileController tile)
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

    private void SelectTiles(IEnumerable<TileController> tilesToSelect)
    {
        UnselectAllTiles();

        foreach (TileController tile in tilesToSelect)
        {
            tile.Select();
        }
    }

    private void UnselectAllTiles()
    {
        foreach (TileController tile in tiles)
        {
            tile.Unselect();
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
        Vector2 startTilePosition = currentTileLine.First().transform.position;
        Vector2 endTilePosition = currentTileLine.Last().transform.position;

        Vector2Int startCoord = currentTileLine.First().Tile.Coord;
        Vector2Int endCoord = currentTileLine.Last().Tile.Coord;
        Directions selectionDirection = GridUtils.CoordDeltaToDirection(startCoord, endCoord);

        List<Vector2Int> truckPath = GetTotalTruckPath(startCoord, endCoord, selectionDirection, false);

        OnTruckTravelCompleted?.Invoke(truckPath);
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
    public PlantCount[] CurrentPathPlants { get => currentPathPlants; }
}