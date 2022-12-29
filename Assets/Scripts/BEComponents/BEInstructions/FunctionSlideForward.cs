using UnityEngine;

public class FunctionSlideForward : BEInstruction
{

    int counterForRepetitions;
    float counterForMovement = 0;
    float movementDuration = 0.5f; //seconds
    Vector3 startPos;
    Vector3 direction;

    public override void BEFunction(BETargetObject targetObject, BEBlock beBlock)
    {
        if (beBlock.beBlockFirstPlay)
        {
            counterForRepetitions = (int)beBlock.BeInputs.numberValues[0];
            startPos = targetObject.transform.position;
            beBlock.beBlockFirstPlay = false;
        }

        if (counterForMovement == 0)
        {
            startPos = targetObject.transform.position;
        }

        if (counterForMovement <= movementDuration)
        {
            counterForMovement += Time.deltaTime;
            if (targetObject.GetComponent<Collider2D>())
            {
                direction = targetObject.transform.right;
            }
            else if(targetObject.GetComponent<Collider>())
            {
                direction = targetObject.transform.forward;
            }
            targetObject.transform.position = Vector3.Lerp(startPos, startPos + direction, counterForMovement / movementDuration);
        }
        else
        {
            counterForMovement = 0;
            counterForRepetitions--;

            if (counterForRepetitions <= 0)
            {
                beBlock.beBlockFirstPlay = true;
                BeController.PlayNextOutside(beBlock);
            }
        }

    }

}
