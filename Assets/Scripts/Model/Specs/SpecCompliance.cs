public class SpecCompliance
{
    public SpecCard[] ValidCards;
    public SpecCard[] GarbageCards;

    public SpecCompliance(SpecCard[] validCards, SpecCard[] garbageCards)
    {
        ValidCards = validCards;
        GarbageCards = garbageCards;
    }
}