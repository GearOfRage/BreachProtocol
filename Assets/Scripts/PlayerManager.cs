using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager _instance;
    public int money = 0;
    private TextMeshProUGUI moneyText;

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void UpdateVisuals()
    {
        moneyText.text = money + " €$";
    }
}
