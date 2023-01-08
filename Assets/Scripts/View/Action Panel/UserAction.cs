using System;
using UnityEngine;

[Serializable]
public abstract class UserAction
{
    [SerializeField]
    protected UserActionTypes actionType;

    public UserAction(UserActionTypes actionType)
    {
        this.actionType = actionType;
    }

    public UserActionTypes ActionType { get => actionType; }
}