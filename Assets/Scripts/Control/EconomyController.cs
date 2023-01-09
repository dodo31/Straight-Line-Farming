using UnityEngine;

public class EconomyController : MonoBehaviour
{
    [SerializeField]
    private EconomyBanner economyPanel;
    
    private Economy economy;

    protected void Awake()
    {
        economy = Economy.GetInstance();
    }
    public void Update()
    {
        if (ShopVars.GetInstance().moneyChange != 0)
        {
            GainMoney(ShopVars.GetInstance().moneyChange);
            ShopVars.GetInstance().moneyChange = 0;
        }
    }

    public int GetMoney()
    {
        return economy.GetMoney();
    }

    public void GainMoney(int amount)
    {
        economy.GainMoney(amount);
        economyPanel.SetAmount(economy.GetMoney());
    }

    public void UseMoney(int amount)
    {
        economy.UseMoney(amount);
        economyPanel.SetAmount(economy.GetMoney());
    }
}