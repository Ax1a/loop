using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Block;

public class BlockOneDrop : BlockDrag
{
    [Header ("Objects")]
    public GameObject dropBlock;
    private int childCount;
    [Header ("Answer")]
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

        if (!string.IsNullOrEmpty(answer)) {
            return consoleValue.ToLower() == answer.ToLower();
        }
        else {
            return true;
        }

    }

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
                    BlockDrag blockDrag = dropBlock.transform.GetChild(0).GetComponent<BlockDrag>();
                    BlockVariable blockVariable = dropBlock.transform.GetChild(0).GetComponent<BlockVariable>();
                    BlockOperator blockOperator = _dropZone.transform.parent.GetComponent<BlockOperator>();
                    if (blockVariable != null) {
                        if (blockVariable.declared) {
                            consoleValue = blockDrag.consoleValue;    
                            error = false;
                        }
                        else if (blockOperator != null && blockOperator.IsCreateVariable()){
                            error = false;
                        }
                        else {
                            error = true;
                        }
                    }
                    else if (blockDrag != null && blockDrag.consoleValue != consoleValue) {
                        consoleValue = blockDrag.consoleValue;
                        error = false;
                    }
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