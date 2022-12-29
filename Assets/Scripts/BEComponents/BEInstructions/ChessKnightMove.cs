using UnityEngine;
using System.Collections;

public class ChessKnightMove : BEInstruction
{
    int counterForRepetitions;
    float counterForMovement = 0;
    float movementDuration = 0.3f; //seconds
    Vector3 startPos;
    int moveSelection = 0;

    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        if (beBlock.beBlockFirstPlay)
        {
            counterForRepetitions = (int)beBlock.BeInputs.numberValues[1];
            beBlock.beBlockFirstPlay = false;
        }

        // [0] first input (dropdown)
        // options: up-right, up-left, down-right, down-left, right-up, right-down, left-up, left-down
        string option = beBlock.BeInputs.stringValues[0];
        string[] options = option.Split('-');

        //movements
        Vector3 firstMovement = GetDirection(options[0]);
        Vector3 secondMovement = GetDirection(options[1]);
        Vector3[] movements = new[] { firstMovement, firstMovement, secondMovement };

        if (counterForMovement == 0)
        {
            startPos = targetObject.transform.position;
        }

        if (counterForMovement <= movementDuration)
        {
            counterForMovement += Time.deltaTime;
            targetObject.transform.position = Vector3.Lerp(startPos, startPos + movements[moveSelection], counterForMovement / movementDuration);
        }
        else
        {
            moveSelection++;
            counterForMovement = 0;
        }

        if(moveSelection == 3)
        {
            moveSelection = 0;
            counterForMovement = 0;
            counterForRepetitions--;

            if (counterForRepetitions <= 0)
            {
                beBlock.beBlockFirstPlay = true;
                BeController.PlayNextOutside(beBlock);
            }
        }
        
    }

    private Vector3 GetDirection(string option)
    {
        switch (option)
        {
            case "up":
                return Vector3.forward;
            case "down":
                return Vector3.back;
            case "right":
                return Vector3.right;
            case "left":
                return Vector3.left;
            default:
                return Vector3.zero;
        }

    }


}
