using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReactiveDefenceProtocol : SecurityProtocol
{
    public override void Effect()
    {
        foreach (GameObject item in GameMaster._instance.matrix)
        {
            item.GetComponent<MatrixValue>().OnClickEvent += BlockValue;
        }
    }

    public void BlockValue(GameObject matrixValue)
    {
        List<GameObject> rndMatrixValues = new List<GameObject>();

        for (int i = 0; i < 2; i++)
        {
            GameObject picked = GameMaster._instance.matrix[
                Random.Range(0, GameMaster.matrixSize),
                Random.Range(0, GameMaster.matrixSize)];

            if (!rndMatrixValues.Contains(picked) && picked.GetComponent<MatrixValue>().value != "[ ]")
            {
                rndMatrixValues.Add(picked);
            }
            else
            {
                i--;
            }
        }

        foreach (GameObject item in rndMatrixValues)
        {
            MatrixValue matrixValueComp = item.GetComponent<MatrixValue>();
            matrixValueComp.b.interactable = false;
            matrixValueComp.UpdateInstance("[ ]", ColorPalette._instance.redLight);
        }
    }
}