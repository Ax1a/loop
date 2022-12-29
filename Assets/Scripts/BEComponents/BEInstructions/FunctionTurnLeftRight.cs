using UnityEngine;
using System.Collections;

public class FunctionTurnLeftRight : BEInstruction
{
    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        Vector3 axis = Vector3.up;

        switch (beBlock.BeInputs.stringValues[0])
        {
            case "Left":
                if (targetObject.GetComponent<Collider2D>())
                {
                    axis = Vector3.forward;
                }
                else if (targetObject.GetComponent<Collider>())
                {
                    axis = Vector3.up;
                }
                targetObject.transform.Rotate(axis, -90);
                break;
            case "Right":
                if (targetObject.GetComponent<Collider2D>())
                {
                    axis = Vector3.forward;
                }
                else if (targetObject.GetComponent<Collider>())
                {
                    axis = Vector3.up;
                }
                targetObject.transform.Rotate(axis, 90);
                break;
            default:
                break;

        }

        BeController.PlayNextOutside(beBlock);
    }

}
