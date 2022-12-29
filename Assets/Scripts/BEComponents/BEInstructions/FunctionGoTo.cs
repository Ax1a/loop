using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FunctionGoTo : BEInstruction
{    
    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        targetObject.transform.position = new Vector3(beBlock.BeInputs.numberValues[0], beBlock.BeInputs.numberValues[1],
            beBlock.BeInputs.numberValues[2]);

        BeController.PlayNextOutside(beBlock);
    }
}
