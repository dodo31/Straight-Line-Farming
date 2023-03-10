using System;
using UnityEngine;

[Serializable]
public class UserSowAction : UserAction
{
    [SerializeField]
    private PlantTypes plantType;

    public UserSowAction(PlantTypes plantType) : base(UserActionTypes.SOW)
    {
        this.plantType = plantType;
    }

    public PlantTypes PlantType { get => plantType; }
}