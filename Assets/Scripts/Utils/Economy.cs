using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Economy : MonoBehaviour
{
    int money;
    [SerializeField]
    public TMP_Text text;
    private static Economy instance;
    public static Economy GetInstance()
    {
        return instance;
    }
    public void GainMoney(int amount)
    {
        money += amount;
        UpdateText();
    }

    public void UseMoney(int amount)
    {
        money -= amount;
        UpdateText();
    }

    public void UpdateText()
    {
        text.text = money.ToString() + "$";
    }

    public int GetMoney()
    {
        return money;
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
}
