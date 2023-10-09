using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicDataExtractionScript : Sequence
{
    [SerializeField] private int defaultMoney = 100;
    private int currentMoney = 100;
    
    public override void Effect()
    {
        PlayerManager._instance.score += currentMoney;
        PlayerManager._instance.UpdateVisuals();
        
    }

    public void ChangeMoney(int newValue)
    {
        currentMoney = newValue;
    }
}
