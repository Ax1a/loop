using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class OperationSum : BEInstruction
{
    string result;
    
    public override string BEOperation(BETargetObject targetObject, BEBlock beBlock)
    {
        if (beBlock.BeInputs.isString)
        {
            result = beBlock.BeInputs.stringValues[0] + beBlock.BeInputs.stringValues[1];
        }
        else
        {
            float tempResult = beBlock.BeInputs.numberValues[0] + beBlock.BeInputs.numberValues[1];
            result = tempResult.ToString(CultureInfo.InvariantCulture);
        }

        return result;
    }
}