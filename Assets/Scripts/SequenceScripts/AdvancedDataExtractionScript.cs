using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedDataExtractionScript : Sequence
{
    [SerializeField] private int defaultMoney = 250;
    private int currentMoney = 250;
    private float restoreTime = 15f;

    public override void Effect()
    {
        PlayerManager player = PlayerManager._instance;
        player.score += currentMoney;
        player.UpdateVisuals();
        
        player.nextTime += Math.Clamp(PlayerManager._instance.nextTime + restoreTime, restoreTime, PlayerManager._instance.defaultTime);
        player.nextTime = Math.Clamp(restoreTime, restoreTime, PlayerManager._instance.defaultTime);
    }
}
