using System;
using UnityEngine;

[Serializable]
public struct TruckSpritePack
{
    public Sprite UpSprite;
    public Sprite UpSideSprite;
    public Sprite DownSprite;
    public Sprite DownSideSprite;

    public OrientedTruckSprite GetSprite(Directions truckDirection)
    {
        switch (truckDirection)
        {
            case Directions.UP:
                return new OrientedTruckSprite(UpSprite, 1);
            case Directions.DOWN:
                return new OrientedTruckSprite(DownSprite, 1);
            case Directions.LEFT_DOWN:
                return new OrientedTruckSprite(DownSideSprite, -1);
            case Directions.LEFT_UP:
                return new OrientedTruckSprite(UpSideSprite, -1);
            case Directions.RIGHT_DOWN:
                return new OrientedTruckSprite(DownSideSprite, 1);
            case Directions.RIGHT_UP:
                return new OrientedTruckSprite(UpSideSprite, 1);
            default:
                return new OrientedTruckSprite(UpSprite, 1);
        }
    }
}