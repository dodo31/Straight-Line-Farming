using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopVars : MonoBehaviour
{
    private static ShopVars instance;
    public int visibleSpecAmount = 1;
    public int gridSize = 0;
    public int baseDays = 5;
    public int moneyChange = 0;
    public int seedPromo = 0;
    public static ShopVars GetInstance()
    {
        return instance;
    }
    public void Awake()
    {
        instance = this;
    }
}
