using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FunctionIf : BEInstruction
{
    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        if (beBlock.BeInputs.stringValues[0] == "1")
        {
            if (beBlock.beChildBlocksList.Count > 0)
            {
                BeController.PlayNextInside(beBlock);
            }
            else
            {
                BeController.PlayNextOutside(beBlock);
            }
        }
        else
        {
            BeController.PlayNextOutside(beBlock);
        }
    }
}
