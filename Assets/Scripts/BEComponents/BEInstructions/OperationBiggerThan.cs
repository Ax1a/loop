using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class OperationBiggerThan : BEInstruction
{
    string result;
    
    public override string BEOperation(BETargetObject targetObject, BEBlock beBlock)
    {
        if (beBlock.BeInputs.isString)
        {
            if (beBlock.BeInputs.stringValues[0].Length > beBlock.BeInputs.stringValues[1].Length)
            {
                result = "1";
            }
            else
            {
                result = "0";
            }
        }
        else
        {
            if (beBlock.BeInputs.numberValues[0] > beBlock.BeInputs.numberValues[1])
            {
                result = "1";
            }
            else
            {
                result = "0";
            }
        }
        
        return result;
    }
}
