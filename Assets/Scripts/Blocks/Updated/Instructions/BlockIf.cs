using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIf : BlockDrag
{
    public GameObject dropBlock;
    public GameObject ifChildContainer;
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
        if (dropBlock.transform.childCount == 0 || !inputChanged) return;
        if (_dropZone == null) return; // Don't check the validation when not on the drop block

        BlockDrag blockDrag = dropBlock.transform.GetChild(0)?.GetComponent<BlockDrag>();
        consoleValue = (blockDrag?.consoleValue == "true") ? "true" : "false";

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                error = false;
                // if (addConsoleValue) consoleValue = inputField.text;

                // if (ValidateInput())
                // {
                    if (!addedPoints && addPoints)
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