using UnityEngine;

public class GridUtils
{
    public const float HOR_SPACING = 1;
    public const float VER_SPACING = 1;

    public static Vector2Int GetGridPosInDirection(Vector2Int gridPosFrom, Directions dir)
    {
        Vector2Int gridPosTo = new();
        if (dir == Directions.LEFT_DOWN || dir == Directions.LEFT_UP)
        {
            gridPosTo.x = gridPosFrom.x - 1;
        }
        else if (dir == Directions.RIGHT_DOWN || dir == Directions.RIGHT_UP)
        {
            gridPosTo.x = gridPosFrom.x + 1;
        }
        else
        {
            gridPosTo.x = gridPosFrom.x;
        }

        if (gridPosFrom.x % 2 == 0 && (dir == Directions.LEFT_UP || dir == Directions.RIGHT_UP))
        {
            gridPosTo.y = gridPosFrom.y - 1;
        }
        else if (gridPosFrom.x % 2 == 1 && (dir == Directions.LEFT_DOWN || dir == Directions.RIGHT_DOWN))
        {
            gridPosTo.y = gridPosFrom.y + 1;
        }

        return gridPosTo;
    }

    public static Directions GetDirectionFromGridPos(Vector2Int fromGridPos, Vector2Int toGridPos)
    {
        int distX = toGridPos.x - fromGridPos.x;
        float distY = toGridPos.y - fromGridPos.y + (1 - toGridPos.x % 2) / 2f - (1 - toGridPos.x % 2) / 2f;
        if (Mathf.Abs(distX) >= Mathf.Abs(distY))
        {
            if (distX > 0)
            {
                if (distY > 0)
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
                if (distY > 0)
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
            if (distY > 0)
            {
                return Directions.UP;
            }
            else
            {
                return Directions.DOWN;
            }
        }
    }

    public static Vector2 GetScreenPosFromGridPos(Vector2Int gridPos)
    {
        return new Vector2
        {
            x = HOR_SPACING * gridPos.x,
            y = VER_SPACING * (gridPos.y + (1 - gridPos.x % 2) / 2f)
        };
    }
}
