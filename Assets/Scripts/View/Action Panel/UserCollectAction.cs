using System;

[Serializable]
public class UserCollectAction : UserAction
{
    public UserCollectAction() : base(UserActionTypes.COLLECT)
    {

    }
}