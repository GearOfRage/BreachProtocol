using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferReloadScript : Sequence
{
    public override void Effect()
    {
        foreach (BufferValue item in GameMaster._instance.buffer.transform)
        {
            item.GetComponent<BufferValue>().ClearValue();
        }

        GameMaster._instance.bufferUsed = 0;
    }
}
