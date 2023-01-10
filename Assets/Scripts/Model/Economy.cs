using UnityEngine;

public class Economy
{
    private int money;

    public Economy()
    {
        money = 0;
    }

    public static Economy GetInstance()
    {
        return EconomyHolder.Instance;
    }

    public void SetMoney(int amount)
    {
        money = amount;
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

    public static class EconomyHolder
    {
        public static Economy Instance = new Economy();
    }
}