using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfessionalDataExtractionScript : Sequence
{
    [SerializeField] private int defaultMoney = 1500;
    private int currentMoney = 1500;

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
