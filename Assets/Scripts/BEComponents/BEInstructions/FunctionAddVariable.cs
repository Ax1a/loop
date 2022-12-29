using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FunctionAddVariable : BEInstruction
{
    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        try
        {
            float newValue = float.Parse(BeController.GetVariable(beBlock.BeInputs.stringValues[0]), CultureInfo.InvariantCulture)
                + beBlock.BeInputs.numberValues[1];
            BeController.SetVariable(beBlock.BeInputs.stringValues[0], newValue.ToString(CultureInfo.InvariantCulture));
        }
        catch
        {
            string newValue = BeController.GetVariable(beBlock.BeInputs.stringValues[0]);
            BeController.SetVariable(beBlock.BeInputs.stringValues[0], newValue.ToString(CultureInfo.InvariantCulture));
        }

        BeController.PlayNextOutside(beBlock);
    }
}
