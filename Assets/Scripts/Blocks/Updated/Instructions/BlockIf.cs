using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIf : BlockDrag
{
    public GameObject dropBlock;
    public GameObject ifChildContainer;
    public string consoleValue;
    private int childCount;

    public override void Update() {
        base.Update();
        
        if (dropBlock.transform.childCount != childCount) {
            childCount = dropBlock.transform.childCount;
            inputChanged = true;
        }
    }

    public override void BlockValidation()
    {
        if (_dropZone == null) return; // Don't check the validation when not on the drop block
        if (dropBlock.transform.childCount == 0) return;

        BlockOperator blockOperator = dropBlock.GetComponent<BlockOperator>();

        if (blockOperator.consoleValue == "true") {
            consoleValue = "true";
        }
        else {
            consoleValue = "false";
        }
    }
}