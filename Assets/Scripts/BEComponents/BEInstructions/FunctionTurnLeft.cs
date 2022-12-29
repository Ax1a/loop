using UnityEngine;
using System.Collections;

public class FunctionTurnLeft : BEInstruction
{
	
    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        Vector3 axis = Vector3.up;

        if (targetObject.GetComponent<Collider2D>())
        {
            axis = Vector3.forward;
        }
        else if (targetObject.GetComponent<Collider>())
        {
            axis = Vector3.up;
        }

        targetObject.transform.Rotate(axis, -90);

        BeController.PlayNextOutside(beBlock);
    }

}
