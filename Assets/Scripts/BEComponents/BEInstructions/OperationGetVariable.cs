using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperationGetVariable : BEInstruction
{
    public override string BEOperation(BETargetObject targetObject, BEBlock beBlock)
    {
        string value = BeController.GetVariable(beBlock.BeInputs.stringValues[0]);

        return value;
    }
}
