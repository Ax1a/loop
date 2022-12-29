using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FunctionMove : BEInstruction
{
    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        if (targetObject.GetComponent<Collider2D>())
        {
            targetObject.transform.position += targetObject.transform.right * beBlock.BeInputs.numberValues[0];
        }
        else if(targetObject.GetComponent<Collider>())
        {
            targetObject.transform.position += targetObject.transform.forward * beBlock.BeInputs.numberValues[0];
        }

        BeController.PlayNextOutside(beBlock);
    }
}
