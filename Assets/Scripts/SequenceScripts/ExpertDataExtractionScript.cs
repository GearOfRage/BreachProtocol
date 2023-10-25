using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpertDataExtractionScript : Sequence
{
    [SerializeField] private int defaultMoney = 500;
    private int currentMoney = 500;

    public override void Effect()
    {
        PlayerManager._instance.score += currentMoney;
        PlayerManager._instance.UpdateVisuals();
        GameMaster._instance.resultPanel.GetComponent<ResultPanelController>().AddExploit();
    }

    public void ChangeMoney(int newValue)
    {
        currentMoney = newValue;
    }
}
