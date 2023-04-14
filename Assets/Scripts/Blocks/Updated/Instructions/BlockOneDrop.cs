using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOneDrop : BlockDrag
{
    public GameObject dropBlock;
    public string consoleValue;
    private int childCount;
    public string answer;


    // public override void Start() {
    //     base.Start();

    //     inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    // }

    private bool ValidateInput()
    {
        if (answer != "" || answer == null) {
            return consoleValue.ToLower() == answer.ToLower();
        }
        else {
            return true;
        }

    }

    // // Function to check if input or dropdown changes
    // // This prevents the block validation for always checking
    // private void OnInputFieldValueChanged(string newValue)
    // {
    //     inputChanged = true;
    // }
    public override void Update() {
        base.Update();
        
        if (dropBlock.transform.childCount != childCount) {
            childCount = dropBlock.transform.childCount;
            inputChanged = true;
        }
    }


    public override void BlockValidation()
    {
        if (_dropZone == null) return; // Don't check the validation when not on the drop block
        if (dropBlock.transform.childCount == 0) {
            consoleValue = "";
            return;
        };
        
        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                if (ValidateInput())
                {
                    if (!addedPoints)
                    {
                        Debug.Log("added points");
                        validationManager.AddPoints(1);
                        addedPoints = true;
                    }
                }
                else
                {
                    if (addedPoints)
                    {
                        Debug.Log("reduced points");
                        validationManager.ReducePoints(1);
                        addedPoints = false;
                    }
                }
                if (blockType == BlockType.Type2) {
                    BlockInput blockInput = dropBlock.transform.GetChild(0).GetComponent<BlockInput>();
                    BlockVariable blockVariable = dropBlock.transform.GetChild(0).GetComponent<BlockVariable>();
                    if (blockInput != null && blockInput.consoleValue.Length != 0 && blockInput.consoleValue != consoleValue) {
                        consoleValue = blockInput.consoleValue;
                    }

                    else if (blockVariable != null && blockVariable.consoleValue.Length != 0 && blockVariable.consoleValue != consoleValue) {
                        consoleValue = blockVariable.consoleValue;
                    }
                }
                else if (blockType == BlockType.Type1) {
                    BlockOneDrop blockDrop = dropBlock.transform.GetChild(0).GetComponent<BlockOneDrop>();
                    BlockOperator blockOperator = dropBlock.transform.GetChild(0).GetComponent<BlockOperator>();

                    if (blockDrop != null && blockDrop.consoleValue.Length != 0 && blockDrop.consoleValue != consoleValue) {
                        consoleValue = blockDrop.consoleValue;
                    }
                    else if (blockOperator != null && blockOperator.consoleValue.Length != 0 && blockOperator.consoleValue != consoleValue) {
                        consoleValue = blockOperator.consoleValue;
                    }
                    // else if (gameObject.name.StartsWith("CharInput")) {
                    //     BlockVariable blockVariable = dropBlock.transform.GetChild(0).GetComponent<BlockVariable>();
                    //     BlockInput blockInput = dropBlock.transform.GetChild(0).GetComponent<BlockInput>();

                    //     if (blockVariable != null) {
                    //         validationManager.AskForInput(blockVariable);
                    //     }
                    //     else if (blockInput != null && blockInput.consoleValue.Length != 0) {
                    //         blockInput.consoleValue = "";
                    //     }
                    // }
                }
                // if (addConsoleValue) consoleValue = inputField.text;
            }
        }

        inputChanged = false;
    }
}