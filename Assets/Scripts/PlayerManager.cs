using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager _instance;
    
    public int score = 0;
    public float defaultTime = 30f;
    public float nextTime = 0;
    public float currentTime;
    
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        currentTime = defaultTime;
    }

    public void UpdateVisuals()
    {
        moneyText.text = score + " €$";
    }
}
