using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FunctionIfColorCollide : BEInstruction
{
    string value = "0";
    
    public override string BEOperation(BETargetObject targetObject, BEBlock beBlock)
    {
        if (targetObject.GetComponent<Collider2D>())
        {
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(targetObject.transform.position, transform.localScale / 2, 0);
            int i = 0;
            while (i < hitColliders.Length)
            {
                value = "0";

                if (hitColliders[i].transform != targetObject.transform)
                {
                    if (beBlock.BeInputs.stringValues[0] == hitColliders[i].GetComponent<Renderer>().sharedMaterial.name)
                    {
                        value = "1";
                        break;
                    }
                }
                i++;
            }
        }
        else if (targetObject.GetComponent<Collider>())
        {
            Collider[] hitColliders = Physics.OverlapBox(targetObject.transform.position, transform.localScale / 2, Quaternion.identity);
            int i = 0;
            while (i < hitColliders.Length)
            {
                value = "0";

                if (hitColliders[i].transform != targetObject.transform)
                {
                    if (beBlock.BeInputs.stringValues[0] == hitColliders[i].GetComponent<Renderer>().sharedMaterial.name)
                    {
                        value = "1";
                        break;
                    }
                }
                i++;
            }
        }

        return value;
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
