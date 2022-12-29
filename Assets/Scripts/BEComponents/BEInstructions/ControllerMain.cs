using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ControllerMain : BEInstruction
{
    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        if(beBlock.BeBlockGroup.isActive)
        {
            BeController.PlayNextInside(beBlock);
        }
    }
}
