using TMPro;
using UnityEngine;

public class EconomyBanner : MonoBehaviour
{
    public TMP_Text amountValueText;

    public void SetAmount(int amount)
    {
        amountValueText.text = amount.ToString();
    }
}