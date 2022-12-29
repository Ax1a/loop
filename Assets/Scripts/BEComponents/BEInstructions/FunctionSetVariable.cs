using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FunctionSetVariable : BEInstruction
{
    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        BeController.SetVariable(beBlock.BeInputs.stringValues[0], beBlock.BeInputs.stringValues[1]);
        BeController.PlayNextOutside(beBlock);
    }
}
