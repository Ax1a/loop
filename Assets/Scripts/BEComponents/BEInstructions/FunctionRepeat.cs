using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FunctionRepeat : BEInstruction
{
    public override string BEOperation(BETargetObject targetObject, BEBlock beBlock)
    {
        if (beBlock.beBlockFirstPlay)
        {
            beBlock.beBlockCounter = (int)beBlock.BeInputs.numberValues[0];
            beBlock.beBlockFirstPlay = false;
        }

        if (beBlock.beBlockCounter > 0)
        {
            beBlock.beBlockCounter--;
            return "1";
        }
        else
        {
            beBlock.beBlockFirstPlay = true;
            return "0";
        }
    }

    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        if (BEOperation(targetObject, beBlock) == "1")
        {
            BeController.PlayNextInside(beBlock);
        }
        else
        {
            BeController.PlayNextOutside(beBlock);
        }
    }
}
