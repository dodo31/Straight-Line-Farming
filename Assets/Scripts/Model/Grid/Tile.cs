using UnityEngine;

public class Tile
{
    private int row;
    private int column;

    private TileTypes type;

    public Tile(int row, int column)
    {
        this.row = row;
        this.column = column;
        this.type = TileTypes.Empty;
    }

    public Vector2Int Coord
    {
        get
        {
            return new Vector2Int(Column, Row);
        }
    }

    public int Row { get => row; set => row = value; }
    public int Column { get => column; set => column = value; }
    public TileTypes Type { get => type; set => type = value; }
}