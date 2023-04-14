using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOneDrop : BlockDrag
{
    public GameObject dropBlock;
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
        if (dropBlock.transform.childCount == 0) {
            consoleValue = "";
            return;
        };
        if (_dropZone == null) return; // Don't check the validation when not on the drop block
        
        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                error = false;
                if (ValidateInput())
                {
                    if (!addedPoints && addPoints)
                    {
                        Debug.Log("added points");
                        validationManager.AddPoints(1);
                        addedPoints = true;
                    }
                }
                else
                {
                    if (addedPoints && addPoints)
                    {
                        Debug.Log("reduced points");
                        validationManager.ReducePoints(1);
                        addedPoints = false;
                    }
                }
                if (blockType == BlockType.Type2) {
                    BlockDrag blockDrag = dropBlock.transform.GetChild(0).GetComponent<BlockDrag>();
                    if (blockDrag != null && blockDrag.consoleValue.Length != 0 && blockDrag.consoleValue != consoleValue) {
                        consoleValue = blockDrag.consoleValue;
                    }
                }
                else if (blockType == BlockType.Type1) {
                    BlockDrag blockDrag = dropBlock.transform.GetChild(0).GetComponent<BlockDrag>();
                    if (blockDrag != null && blockDrag.consoleValue.Length != 0 && blockDrag.consoleValue != consoleValue) {
                        consoleValue = blockDrag.consoleValue;
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
                return;
            }
            else {
                error = true;
            }
        }

        inputChanged = false;
    }
}