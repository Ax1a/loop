using UnityEngine;
using System.Collections;
using System.Globalization;

public class OperationNot : BEInstruction
{
    string result;

    public override string BEOperation(BETargetObject targetObject, BEBlock beBlock)
    {
        string inputString = beBlock.BeInputs.stringValues[0];
		
        if (inputString == "0" || inputString == "False" || inputString == "false")
        {
            result = "1";
        }
        else if (inputString == "1" || inputString == "True" || inputString == "true")
        {
            result = "0";
        }
        else if (beBlock.BeInputs.isString)
        {
			//reverse string
			char[] charArray = inputString.ToCharArray();
            System.Array.Reverse( charArray );
			result = new string( charArray );
        }
        else
        {
			//invert number
            float tempResult = beBlock.BeInputs.numberValues[0] * -1;
            result = tempResult.ToString(CultureInfo.InvariantCulture);
        }

        return result;
    }

}
