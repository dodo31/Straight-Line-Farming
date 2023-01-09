using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    [SerializeField]
    ActionItem shopPromos;
    [SerializeField]
    ActionItem shopSpecsAdd;
    [SerializeField]
    ActionItem shopTimeAdd;
    [SerializeField]
    ActionItem shopLife;
    [SerializeField]
    ActionItem shopBackInTime;
    [SerializeField]
    ActionItem shopTurn;
    [SerializeField]
    private EconomyController economyController;
    [SerializeField]
    private SpecCardsContainer specCardsContainer;

    int[] shopPromoPrices = { 350, 800, 1600 };
    int[] shopSpecsAddPrices = { 200, 600, 1000 };
    int[] shopTimeAddPrices = { 300, 700, 1200 };
    int shopLifePrice = 1000;
    int shopBackInTimePrice = 1500;
    int[] shopTurnPrices = { 5000 };

    int currentPromo = 0;
    int currentSpecsAdd = 0;
    int currentTimeAdd = 0;
    int currentTurn = 0;

    public void Start()
    {
        shopPromos.OnClicked += ShopPromosClicked;
        shopSpecsAdd.OnClicked += ShopSpecsAddClicked;
        shopTimeAdd.OnClicked += ShopTimeAddClicked;
        shopLife.OnClicked += ShopLifeClicked;
        shopBackInTime.OnClicked += ShopBackInTimeClicked;
        shopTurn.OnClicked += ShopTurnClicked;
        shopPromos.SetPrice(shopPromoPrices[0]);
        shopSpecsAdd.SetPrice(shopSpecsAddPrices[0]);
        shopTimeAdd.SetPrice(shopTimeAddPrices[0]);
        shopLife.SetPrice(shopLifePrice);
        shopBackInTime.SetPrice(shopBackInTimePrice);
        shopTurn.SetPrice(shopTurnPrices[0]);

    }

    public void Update()
    {
        shopPromos.SetAvailable(shopPromoPrices.Length > currentPromo && Economy.GetInstance().GetMoney() >= shopPromoPrices[currentPromo]);
        shopSpecsAdd.SetAvailable(shopSpecsAddPrices.Length > currentSpecsAdd && Economy.GetInstance().GetMoney() >= shopSpecsAddPrices[currentSpecsAdd]);
        shopTimeAdd.SetAvailable(shopTimeAddPrices.Length > currentTimeAdd && Economy.GetInstance().GetMoney() >= shopTimeAddPrices[currentTimeAdd]);
        shopLife.SetAvailable(Economy.GetInstance().GetMoney() >= shopLifePrice && ShopVars.GetInstance().lives <3);
        
        shopBackInTime.SetAvailable(Economy.GetInstance().GetMoney() >= shopBackInTimePrice && specCardsContainer.LowestDeadlineSpecCard() <ShopVars.GetInstance().baseDays);

        shopTurn.SetAvailable(shopTurnPrices.Length > currentTurn && Economy.GetInstance().GetMoney() >= shopTurnPrices[currentTurn]);
    }
    public void ShopPromosClicked(ActionItem item)
    {
        if (currentPromo >= shopPromoPrices.Length) return;
        int price = shopPromoPrices[currentPromo];
        if (economyController.GetMoney() < price)
            return;
        economyController.UseMoney(price);
        currentPromo++;
        ShopVars.GetInstance().seedPromo++;
        if (shopPromoPrices.Length > currentPromo)
            shopPromos.SetPrice(shopPromoPrices[currentPromo]);
        else Destroy(shopPromos.gameObject);
    }
    public void ShopSpecsAddClicked(ActionItem item)
    {
        if (currentSpecsAdd >= shopSpecsAddPrices.Length) return;
        int price = shopSpecsAddPrices[currentSpecsAdd];
        if (economyController.GetMoney() < price)
            return;
        economyController.UseMoney(price);
        currentSpecsAdd++;
        ShopVars.GetInstance().visibleSpecAmount++;
        if (shopSpecsAddPrices.Length > currentSpecsAdd)
            shopSpecsAdd.SetPrice(shopSpecsAddPrices[currentSpecsAdd]);
        else Destroy(shopSpecsAdd.gameObject);

    }
    public void ShopTimeAddClicked(ActionItem item)
    {
        if (currentTimeAdd >= shopTimeAddPrices.Length) return;
        int price = shopTimeAddPrices[currentTimeAdd];
        if (economyController.GetMoney() < price)
            return;
        economyController.UseMoney(price);
        currentTimeAdd++;
        ShopVars.GetInstance().baseDays++;
        if (shopTimeAddPrices.Length > currentTimeAdd)
            shopTimeAdd.SetPrice(shopTimeAddPrices[currentTimeAdd]);
        else Destroy(shopTimeAdd.gameObject);
    }
    public void ShopLifeClicked(ActionItem item)
    {
        if (ShopVars.GetInstance().lives >= 3) return;
        int price = shopLifePrice;
        if (economyController.GetMoney() < price)
            return;
        economyController.UseMoney(price);
        ShopVars.GetInstance().lives++;
    }
    public void ShopBackInTimeClicked(ActionItem item)
    {
        if (specCardsContainer.LowestDeadlineSpecCard() >= ShopVars.GetInstance().baseDays) return;
        int price = shopBackInTimePrice;
        if (economyController.GetMoney() < price)
            return;
        economyController.UseMoney(price);
        specCardsContainer.GoBackInTime();
    }
    public void ShopTurnClicked(ActionItem item)
    {
        if (currentTurn >= shopTurnPrices.Length) return;
        int price = shopTurnPrices[currentTurn];
        if (economyController.GetMoney() < price)
            return;
        economyController.UseMoney(price);
        currentTurn++;
        Debug.Log("end of the game here!");
        if (shopTurnPrices.Length > currentTurn)
            shopTurn.SetPrice(shopTurnPrices[currentTurn]);
        else Destroy(shopTurn.gameObject);
    }

}
