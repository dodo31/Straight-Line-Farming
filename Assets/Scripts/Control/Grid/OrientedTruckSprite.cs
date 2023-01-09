using System;
using UnityEngine;

[Serializable]
public struct OrientedTruckSprite
{
    public Sprite Sprite;
    public int Direction;

    public OrientedTruckSprite(Sprite sprite, int direction)
    {
       Sprite = sprite;
       Direction = direction; 
    }
}