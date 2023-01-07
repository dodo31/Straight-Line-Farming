using System.Collections.Generic;

public class Grid
{
    private const int SIZE = 9;

    private List<List<Tile>> tiles;

    public Grid()
    {
        tiles = new List<List<Tile>>();

        for (int column = 0; column < SIZE; column++)
        {
            List<Tile> tileColumn = new List<Tile>();

            for (int row = 0; row < SIZE; row++)
            {
                Tile newTile = new Tile(column, row);
                tileColumn.Add(newTile);
            }

            tiles.Add(tileColumn);
        }
    }

    public int RowCount
    {
        get
        {
            return tiles.Count;
        }
    }

    public int ColumnCount
    {
        get
        {
            if (tiles.Count > 0)
            {
                return tiles[0].Count;
            }
            else
            {
                return 0;
            }
        }
    }

    public List<List<Tile>> Tiles { get => tiles; set => tiles = value; }
}