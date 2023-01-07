using UnityEngine;

public class GridUtils
{
    public enum Direction
    {
        DOWN,
        UP,
        LEFT_DOWN,
        LEFT_UP,
        RIGHT_DOWN,
        RIGHT_UP
    }
    public const float HOR_SPACE_SCREEN_POS = 0;
    public const float VER_SPACE_SCREEN_POS = 0;
    public static Vector2Int GetGridPosInDirection(Vector2Int gridPosFrom, Direction dir)
    {
        Vector2Int gridPosTo = new();
        if (dir == Direction.LEFT_DOWN || dir == Direction.LEFT_UP)
        {
            gridPosTo.x = gridPosFrom.x - 1;
        }
        else if (dir == Direction.RIGHT_DOWN || dir == Direction.RIGHT_UP)
        {
            gridPosTo.x = gridPosFrom.x + 1;
        }
        else
        {
            gridPosTo.x = gridPosFrom.x;
        }

        if (gridPosFrom.x % 2 == 0 && (dir == Direction.LEFT_UP || dir == Direction.RIGHT_UP))
        {
            gridPosTo.y = gridPosFrom.y - 1;
        }
        else if (gridPosFrom.x % 2 == 1 && (dir == Direction.LEFT_DOWN || dir == Direction.RIGHT_DOWN))
        {
            gridPosTo.y = gridPosFrom.y + 1;
        }

        return gridPosTo;
    }

    public static Direction GetDirectionFromGridPos(Vector2Int fromGridPos, Vector2Int toGridPos)
    {
        int distX = toGridPos.x - fromGridPos.x;
        float distY = toGridPos.y - fromGridPos.y + (1 - toGridPos.x % 2) / 2f - (1 - toGridPos.x % 2) / 2f;
        if (Mathf.Abs(distX) >= Mathf.Abs(distY))
        {
            if (distX > 0)
            {
                if (distY > 0)
                {
                    return Direction.RIGHT_UP;
                }
                else
                {
                    return Direction.RIGHT_DOWN;
                }
            }
            else
            {
                if (distY > 0)
                {
                    return Direction.LEFT_UP;
                }
                else
                {
                    return Direction.LEFT_DOWN;
                }
            }
        }
        else
        {
            if (distY > 0)
            {
                return Direction.UP;
            }
            else
            {
                return Direction.DOWN;
            }
        }
    }

    public static Vector2 GetScreenPosFromGridPos(Vector2Int gridPos)
    {
        return new Vector2(HOR_SPACE_SCREEN_POS * gridPos.x, VER_SPACE_SCREEN_POS * (gridPos.y + (1 - gridPos.x % 2) / 2f));
    }
}
