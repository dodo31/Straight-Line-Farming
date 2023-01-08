using System;
using UnityEngine;

[Serializable]
public class UserAction
{
    [SerializeField]
    private UserActionTypes actionType;

    public UserAction(UserActionTypes actionType)
    {
        this.actionType = actionType;
    }

    protected UserActionTypes ActionType { get => actionType; }
}