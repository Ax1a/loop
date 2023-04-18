using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockOneDrop : BlockDrag
{
    public GameObject dropBlock;
    private int childCount;
    public string answer;
    private string dropConsoleValue;

    private bool ValidateInput()
    {
        string[] answerLines = answer.Split(new string[] { "|" }, StringSplitOptions.None);
        if (answerLines.Length > 1) {
            foreach (var answer in answerLines)
            {
                if (consoleValue.ToLower() == answer.ToLower()) return true;
            }

            return false;
        }

        if (answer != "") {
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
        
        if (dropBlock.transform.childCount == 0) {
            error = true;
            inputChanged = false;
        }

        if (dropBlock.transform.childCount != childCount) {
            childCount = dropBlock.transform.childCount;
            inputChanged = true;
        }

        if (dropBlock.transform.childCount > 0 && dropBlock.transform.GetChild(0).GetComponent<BlockDrag>().consoleValue != dropConsoleValue) {
            dropConsoleValue = dropBlock.transform.GetChild(0).GetComponent<BlockDrag>().consoleValue;
            inputChanged = true;
        }
    }


    public override void BlockValidation()
    {
        if (_dropZone == null || !inputChanged) return; // Don't check the validation when not on the drop block
        
        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                error = false;
                if (dropBlock.transform.childCount > 0) {
                    // if (blockType == BlockType.Type2) {
                        BlockDrag blockDrag = dropBlock.transform.GetChild(0).GetComponent<BlockDrag>();
                        if (blockDrag != null && blockDrag.consoleValue != consoleValue) {
                            consoleValue = blockDrag.consoleValue;
                        }
                    // }
                    // else if (blockType == BlockType.Type1) {
                        // BlockDrag blockDrag = dropBlock.transform.GetChild(0).GetComponent<BlockDrag>();
                        // if (blockDrag != null && blockDrag.consoleValue.Length != 0 && blockDrag.consoleValue != consoleValue) {
                        //     consoleValue = blockDrag.consoleValue;
                        // }
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
                    // }
                }
                else {
                    inputChanged = false;
                    consoleValue = "";
                    return;
                }

                if (ValidateInput())
                {
                    if (!addedPoints && addPoints)
                    {
                        validationManager.AddPoints(1);
                        addedPoints = true;
                    }
                }
                else
                {
                    if (addedPoints && addPoints)
                    {
                        validationManager.ReducePoints(1);
                        addedPoints = false;
                    }
                }

                inputChanged = false;
                return;
            }
            else {
                error = true;
            }
        }

        inputChanged = false;
    }
}