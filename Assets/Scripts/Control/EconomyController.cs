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