using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionChangeColor : BEInstruction
{
    Material mat;
    
    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        if (beBlock.BeInputs.stringValues[0] == "Random")
        {
            targetObject.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
        else
        {
            mat = BeController.GetColor(beBlock.BeInputs.stringValues[0]);
            targetObject.GetComponent<Renderer>().material.color = mat.color;
        }

        BeController.PlayNextOutside(beBlock);
    }
}
