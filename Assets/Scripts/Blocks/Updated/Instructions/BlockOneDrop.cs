using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOneDrop : BlockDrag
{
    public GameObject dropBlock;
    // public string answer;


    // public override void Start() {
    //     base.Start();

    //     inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    // }

    // private bool ValidateInput()
    // {
    //     return inputField.text.ToLower() == answer.ToLower();
    // }

    // // Function to check if input or dropdown changes
    // // This prevents the block validation for always checking
    // private void OnInputFieldValueChanged(string newValue)
    // {
    //     inputChanged = true;
    // }

    public override void BlockValidation()
    {
        Debug.Log(blockType);
        if (_dropZone == null || !inputChanged) return; // Don't check the validation when not on the drop block

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                // if (addConsoleValue) consoleValue = inputField.text;

                // if (ValidateInput())
                // {
                //     if (!addedPoints)
                //     {
                //         validationManager.AddPoints(1);
                //         addedPoints = true;
                //     }
                // }
                // else
                // {
                //     if (addedPoints)
                //     {
                //         validationManager.ReducePoints(1);
                //         addedPoints = false;
                //     }
                // }
            }
        }

        inputChanged = false;
    }
}