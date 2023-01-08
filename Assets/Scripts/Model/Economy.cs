public class Economy
{
    private int money;

    private Economy()
    {
        money = 0;
    }

    public static Economy GetInstance()
    {
        return EconomyHolder.Instance;
    }

    public void GainMoney(int amount)
    {
        money += amount;
    }

    public void UseMoney(int amount)
    {
        money -= amount;
    }

    public int GetMoney()
    {
        return money;
    }

    private static class EconomyHolder
    {
        public static Economy Instance = new Economy();
    }
}