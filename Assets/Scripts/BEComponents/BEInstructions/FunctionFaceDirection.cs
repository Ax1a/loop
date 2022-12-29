using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class FunctionFaceDirection : BEInstruction
{
    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        if (targetObject.GetComponent<Collider2D>())
        {
            switch (beBlock.BeInputs.stringValues[0])
            {
                case "Forward":
                    targetObject.transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case "Back":
                    targetObject.transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
                case "Up":
                    targetObject.transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
                case "Down":
                    targetObject.transform.eulerAngles = new Vector3(0, 0, -90);
                    break;
                case "Left":
                    targetObject.transform.eulerAngles = new Vector3(0, 0, 180);
                    break;
                case "Right":
                    targetObject.transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                default:
                    targetObject.transform.eulerAngles = new Vector3(0, 0, 90);
                    break;
            }
        }
        else if(targetObject.GetComponent<Collider>())
        {
            switch (beBlock.BeInputs.stringValues[0])
            {
                case "Forward":
                    targetObject.transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
                case "Back":
                    targetObject.transform.eulerAngles = new Vector3(0, 180, 0);
                    break;
                case "Up":
                    targetObject.transform.eulerAngles = new Vector3(-90, 0, 0);
                    break;
                case "Down":
                    targetObject.transform.eulerAngles = new Vector3(90, 0, 0);
                    break;
                case "Left":
                    targetObject.transform.eulerAngles = new Vector3(0, -90, 0);
                    break;
                case "Right":
                    targetObject.transform.eulerAngles = new Vector3(0, 90, 0);
                    break;
                default:
                    targetObject.transform.eulerAngles = new Vector3(0, 0, 0);
                    break;
            }
        }

        BeController.PlayNextOutside(beBlock);
    }
    
}
