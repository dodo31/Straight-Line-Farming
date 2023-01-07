public struct PlantCount
{
    public PlantTypes Type;
    public int Count;

    public PlantCount(PlantTypes type, int count)
    {
        Type = type;
        Count = count;
    }
}