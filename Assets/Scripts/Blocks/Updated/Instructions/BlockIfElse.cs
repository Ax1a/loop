using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockIfElse : BlockDrag
{
    public GameObject dropBlock;
    public GameObject ifChildContainer;
    public GameObject elseChildContainer;
    [SerializeField] private string answer;
    BlockDrag blockDrag = null;

    public override void Update() {
        base.Update();
        
        if (dropBlock.transform.childCount > 0) {
            blockDrag = dropBlock.transform.GetChild(0)?.GetComponent<BlockDrag>();

            if (blockDrag.consoleValue != consoleValue) {
                consoleValue = blockDrag.consoleValue;
                inputChanged = true;
            }
        }
    }

    private bool ValidateInput() {
        return answer.ToLower() == consoleValue.ToLower();
    }

    public override void BlockValidation()
    {
        if (_dropZone == null) return; // Don't check the validation when not on the drop block
        if (dropBlock.transform.childCount == 0 || !inputChanged) return;

        blockDrag = dropBlock.transform.GetChild(0)?.GetComponent<BlockDrag>();
        consoleValue = (blockDrag?.consoleValue == "true") ? "true" : "false";

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                error = false;

                if (ValidateInput())
                {
                    if (!addedPoints)
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

        inputChanged = false;
    }
}