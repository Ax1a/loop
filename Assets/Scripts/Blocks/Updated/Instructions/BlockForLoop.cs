using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockForLoop : BlockDrag
{
    public GameObject childContainer;
    public GameObject conditionValue;
    public TMP_InputField variableValue;
    public TMP_Dropdown conditionOperator;
    public TMP_Dropdown assignOperator;

    private int childCount;

    public override void Update() {
        base.Update();
        
        // if (dropBlock.transform.childCount != childCount) {
        //     childCount = dropBlock.transform.childCount;
        //     inputChanged = true;
        // }
    }

    public override void BlockValidation()
    {
        if (_dropZone == null) return; // Don't check the validation when not on the drop block
        // if (dropBlock.transform.childCount == 0) return;

        // BlockOperator blockOperator = dropBlock.GetComponent<BlockOperator>();

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
            }
        }
    }
}