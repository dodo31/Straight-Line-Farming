using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateDay : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = "Day " + ShopVars.GetInstance().day;
        SceneController.daysToWin = ShopVars.GetInstance().day;
    }
}
