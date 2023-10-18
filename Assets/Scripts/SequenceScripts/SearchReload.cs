using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchReload : Sequence
{
    private int time = 20;

    public override void Effect()
    {
        PlayerManager._instance.nextTime = PlayerManager._instance.defaultTime;
        
    }
}
