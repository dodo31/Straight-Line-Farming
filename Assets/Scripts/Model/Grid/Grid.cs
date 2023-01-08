using System.Collections.Generic;
using UnityEngine;

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

    public Tile GetTile(Vector2Int coord)
    {
        if (coord.x >= 0 && coord.x < tiles.Count)
        {
            List<Tile> matchingColumn = tiles[coord.x];

            if (coord.y >= 0 && coord.y < matchingColumn.Count)
            {
                return matchingColumn[coord.y];
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
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