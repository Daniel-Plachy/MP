using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class HUDMoneyDisplay : MonoBehaviour
{
    public TMP_Text goldText;


    void Update()
    {
        if (MoneySystem.instance != null)
        {
            goldText.text = ": " + MoneySystem.instance.Gold.ToString();
        }
        else
        {
            goldText.text = ": 0";
        }
    }
}
