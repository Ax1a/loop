using UnityEngine;
using System.Collections;

public class OperationNand : BEInstruction
{
    string result;

    public override string BEOperation(BETargetObject targetObject, BEBlock beBlock)
    {
        string inputString0 = beBlock.BeInputs.stringValues[0];
        string inputString1 = beBlock.BeInputs.stringValues[1];

        result = !((inputString0 == "1" || inputString0 == "True" || inputString0 == "true") && (inputString1 == "1" || inputString1 == "True" || inputString1 == "true"))
        ? "1" : "0";

        return result;
    }
}
