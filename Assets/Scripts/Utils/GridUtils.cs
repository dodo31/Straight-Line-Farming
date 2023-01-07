using UnityEngine;

public class GridUtils
{
    public const float TILES_WIDTH = 1.0f;
    public const float TILES_HEIGHT = 0.85f;

    public static Vector2Int CoordDirectionToCoordDelta(Vector2Int startCoord, Directions direction)
    {
        Vector2Int gridPosTo = new();
        
        if (direction == Directions.LEFT_DOWN || direction == Directions.LEFT_UP)
        {
            gridPosTo.x = startCoord.x - 1;
        }
        else if (direction == Directions.RIGHT_DOWN || direction == Directions.RIGHT_UP)
        {
            gridPosTo.x = startCoord.x + 1;
        }
        else
        {
            gridPosTo.x = startCoord.x;
        }

        if (startCoord.x % 2 == 0 && (direction == Directions.LEFT_UP || direction == Directions.RIGHT_UP))
        {
            gridPosTo.y = startCoord.y - 1;
        }
        else if (startCoord.x % 2 == 1 && (direction == Directions.LEFT_DOWN || direction == Directions.RIGHT_DOWN))
        {
            gridPosTo.y = startCoord.y + 1;
        }
        else gridPosTo.y = startCoord.y;

        return gridPosTo;
    }

    public static Directions CoordDeltaToDirection(Vector2Int startCoord, Vector2Int endCoord)
    {
        int deltaX = endCoord.x - startCoord.x;
        float deltaY = endCoord.y - startCoord.y + (1 - endCoord.x % 2) / 2f - (1 - endCoord.x % 2) / 2f;
        
        if (Mathf.Abs(deltaX) >= Mathf.Abs(deltaY))
        {
            if (deltaX > 0)
            {
                if (deltaY > 0)
                {
                    return Directions.RIGHT_UP;
                }
                else
                {
                    return Directions.RIGHT_DOWN;
                }
            }
            else
            {
                if (deltaY > 0)
                {
                    return Directions.LEFT_UP;
                }
                else
                {
                    return Directions.LEFT_DOWN;
                }
            }
        }
        else
        {
            if (deltaY > 0)
            {
                return Directions.UP;
            }
            else
            {
                return Directions.DOWN;
            }
        }
    }

    public static Vector2 CoordToScreenPosition(Vector2Int coord)
    {
        return new Vector2
        {
            x = TILES_WIDTH * coord.x,
            y = TILES_HEIGHT * (coord.y + (1 - coord.x % 2) / 2f)
        };
    }
}
