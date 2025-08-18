using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferReloadScript : Sequence
{
    public override void Effect()
    {
        List<GameObject> bufferValuesGO = Utility.GetChildren(GameMaster._instance.buffer);
        foreach (GameObject item in bufferValuesGO)
        {
            BufferValue bv = item.GetComponent<BufferValue>();
            bv.ClearValue();
        }
        GameMaster._instance.MoveSequenceHighlight();
        GameMaster._instance.bufferUsed = 0;
    }
}
