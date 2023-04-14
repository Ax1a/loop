using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIfElse : BlockDrag
{
    public GameObject dropBlock;
    public GameObject ifChildContainer;
    public GameObject elseChildContainer;
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

        // if (blockOperator.consoleValue == "true") {
        //     consoleValue = "true";
        // }
        // else {
        //     consoleValue = "false";
        // }

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                // if (addConsoleValue) consoleValue = inputField.text;

                // if (ValidateInput())
                // {
                    if (!addedPoints)
                    {
                        validationManager.AddPoints(1);
                        addedPoints = true;
                    }
                // }
                // else
                // {
                    // if (addedPoints)
                    // {
                    //     validationManager.ReducePoints(1);
                    //     addedPoints = false;
                    // }
                // }
                return;
            }
            else {
                error = true;
            }
        }
    }
}