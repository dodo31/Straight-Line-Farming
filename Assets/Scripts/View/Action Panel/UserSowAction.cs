public class UserSowAction : UserAction
{
    private PlantTypes plantType;

    public UserSowAction(PlantTypes plantType) : base(UserActionTypes.SOW)
    {
        this.plantType = plantType;
    }

    public PlantTypes PlantType { get => plantType; }
}