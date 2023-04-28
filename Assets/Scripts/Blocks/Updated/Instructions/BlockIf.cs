using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIf : BlockDrag
{
    [Header ("Objects")]
    public GameObject dropBlock;
    public GameObject ifChildContainer;
    
    [Header ("Answer")]
    [SerializeField] private string answer;
    BlockDrag blockDrag = null;

    public override void Update() {
        base.Update();

        if (dropBlock.transform.childCount > 0) {
            blockDrag = dropBlock.transform.GetChild(0)?.GetComponent<BlockDrag>();

            if (blockDrag.consoleValue != consoleValue) {
                inputChanged = true;
            }
        }
    }

    private bool ValidateInput() {
        if (answer != "") {
            return answer.ToLower() == consoleValue.ToLower();
        }
        else {
            return true;
        }
    }

    public override void BlockValidation()
    {
        if (dropBlock.transform.childCount == 0 || !inputChanged) return;
        if (_dropZone == null) return; // Don't check the validation when not on the drop block

        if (blockDrag?.consoleValue == "true" && !blockDrag.error) {
            error = false;
            consoleValue = "true";
        }
        else if (blockDrag?.consoleValue == "false" && !blockDrag.error) {
            error = false;
            consoleValue = "false";
        }
        else {
            consoleValue = "";
            error = true;
        }

        if (!error) {
            foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
            {
                if (dropID == id)
                {
                    error = false;

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
                        if (addedPoints)
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
        }

        inputChanged = false;
    }
}