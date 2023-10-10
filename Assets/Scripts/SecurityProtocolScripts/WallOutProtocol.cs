using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOutProtocol : SecurityProtocol
{
    public override void Effect()
    {
        GameObject[,] mtx = GameMaster._instance.matrix;
        int blockOffset = 1;

        
        foreach (GameObject item in mtx)
        {
            MatrixValue matrixValue = item.GetComponent<MatrixValue>();
            if (matrixValue.column <= blockOffset)
            {
                matrixValue.b.interactable = false;
                matrixValue.UpdateInstance("[ ]", ColorPalette._instance.redLight);
            }
        }
    }
}