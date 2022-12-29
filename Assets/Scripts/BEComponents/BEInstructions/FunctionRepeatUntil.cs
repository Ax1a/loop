using UnityEngine;
using System.Collections;

public class FunctionRepeatUntil : BEInstruction
{

    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        if (beBlock.BeInputs.stringValues[0] == "1")
        {
            BeController.PlayNextOutside(beBlock);
        }
        else
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
    }

}
