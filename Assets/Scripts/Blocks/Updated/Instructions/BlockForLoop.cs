using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockForLoop : BlockDrag
{
    public string consoleValue;
    public GameObject childContainer;
    public TMP_InputField variableValue;
    public TMP_InputField conditionValue;
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
    }
}