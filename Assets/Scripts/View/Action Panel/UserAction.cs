public abstract class UserAction
{
    private UserActionTypes actionType;

    public UserAction(UserActionTypes actionType)
    {
        this.actionType = actionType;
    }

    protected UserActionTypes ActionType { get => actionType; }
}