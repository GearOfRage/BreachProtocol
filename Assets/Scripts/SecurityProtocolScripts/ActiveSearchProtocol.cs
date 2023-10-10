using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSearchProtocol : SecurityProtocol
{
    public override void Effect()
    {
        StartCoroutine(TimerController._instance.StartCountdown());
    }
}
