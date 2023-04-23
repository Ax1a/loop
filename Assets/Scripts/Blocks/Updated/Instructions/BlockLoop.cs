using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLoop : BlockDrag
{
    public GameObject dropBlock;
    public GameObject childContainer;
    [SerializeField] private int maxLoop = 50;
    [SerializeField] private LoopType loopType;
    private enum LoopType { While, DoWhile }
    private int childCount;
    private int ctr = 0;
    private BlockDrag blockDrag = null;


    public override void Update() {
        base.Update();
        
        if (dropBlock.transform.childCount != childCount) {
            childCount = dropBlock.transform.childCount;
            inputChanged = true;
        }
    }

    private IEnumerator UpdateBlocks(Transform parent) {
        foreach (Transform child in parent) {
            if (child.GetComponent<BlockDrag>() != null) {
                BlockDrag _blockDrag = child.GetComponent<BlockDrag>();
                _blockDrag.inputChanged = true;
            }

            yield return UpdateBlocks(child);
        }
    }
    
    // public void LoopChildBlocks() {
    //     if (childContainer.transform.childCount > 0) {

    //         foreach (Transform child in childContainer.transform) {
    //             BlockDrag blockDragChild = child.GetComponent<BlockDrag>();
    //             BlockOperator blockOperator = child.GetComponent<BlockOperator>();
    //             if (blockOperator != null) {
    //                 blockOperator.IncrementValue();
    //             }                

    //             if (blockDragChild != null && blockDragChild.printConsole) {
    //                 // Debug.Log(child.name + " " + blockDragChild.consoleValue);
    //                 validationManager._consoleLog += blockDragChild.consoleValue + "\n";
    //             }
    //         }
    //         BlockOperator headerOperator = dropBlock.transform.GetChild(0).GetComponent<BlockOperator>();
    //         if (headerOperator != null) {
    //             headerOperator.ExecuteOperator();
    //         }
    //         CheckHeaderOperator();
    //     }

    //     ctr++;
    // }

    public IEnumerator LoopChildBlocks(Transform childContainer) {
        if (childContainer.transform.childCount > 0) {
            // yield return StartCoroutine(UpdateBlocks(childContainer));
            foreach (Transform child in childContainer) {
                if (child.name.Equals("IfCondition")) {
                    if (child.parent.GetComponent<BlockDrag>()?.consoleValue == "false") continue;
                }
                else if (child.name.Equals("ElseCondition")) {
                    if (child.parent.GetComponent<BlockDrag>()?.consoleValue == "true") continue;
                }
                
                if (child.GetComponent<BlockDrag>() != null) {
                    BlockDrag blockDragChild = child.GetComponent<BlockDrag>();
                    BlockOperator blockOperator = child.GetComponent<BlockOperator>();
                    
                    if (child.name.StartsWith("C_IfCondition") && !blockDragChild.consoleValue.ToLower().Equals("true") ||
                        child.name.StartsWith("J_IfCondition") && !blockDragChild.consoleValue.ToLower().Equals("true") ||
                        child.name.StartsWith("P_IfCondition") && !blockDragChild.consoleValue.ToLower().Equals("true"))
                    {
                        continue;
                    }

                    if (blockOperator != null) {
                        blockOperator.IncrementValue();
                    }

                    if (blockDragChild.printConsole && blockDragChild.blockType == BlockType.Type1) {
                        validationManager._consoleLog += blockDragChild.consoleValue + "\n";
                    }
                }
                
                yield return LoopChildBlocks(child);
            }

            BlockOperator headerOperator = dropBlock.transform.GetChild(0).GetComponent<BlockOperator>();
            if (headerOperator != null) {
                headerOperator.ExecuteOperator();
            }
            CheckHeaderOperator();
        }
    }

    public IEnumerator DelayLoop() {
        CheckHeaderOperator();
        ctr = 0;
        if (loopType == LoopType.While) {
            while (consoleValue == "true" && ctr < maxLoop) {
                yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                yield return new WaitForSeconds(0.1f);
                yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                yield return new WaitForSeconds(0.1f);
                Debug.Log(consoleValue);
                ctr++;
            }
        }
        else if (loopType == LoopType.DoWhile) {
            do {
                yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                yield return new WaitForSeconds(0.1f);
                yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                yield return new WaitForSeconds(0.1f);
                Debug.Log(consoleValue);
                ctr++;
            }
            while (consoleValue == "true" && ctr < maxLoop);
        }

        // Remove the last line from the console log
        // string[] consoleLogLines = validationManager._consoleLog.Split('\n');
        // if (consoleLogLines.Length > 1) {
        //     validationManager._consoleLog = string.Join("\n", consoleLogLines, 0, consoleLogLines.Length - 2);
        //     validationManager._consoleLog += "\n";
        // }
        
        StartCoroutine(validationManager.DelayDisable());
    }

    private void CheckHeaderOperator() {
        blockDrag = dropBlock.transform.GetChild(0)?.GetComponent<BlockDrag>();

        if (blockDrag.consoleValue == "true") {
            consoleValue = "true";
        }
        else {
            consoleValue = "false";
        }
    }

    public override void BlockValidation()
    {
        if (_dropZone == null || instantiate || inputChanged) return; // Don't check the validation when not on the drop block
        if (dropBlock.transform.childCount == 0) return;

        CheckHeaderOperator();

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                if (consoleValue != ""){
                    // StartCoroutine(DelayLoop());
                    // return;
                }
                // if (addConsoleValue) consoleValue = inputField.text;

                // if (ValidateInput())
                // {
                    if (!addedPoints)
                    {
                        validationManager.AddPoints(1);
                        addedPoints = true;
                    }
                // }
                // else
                // {
                    // if (addedPoints)
                    // {
                    //     validationManager.ReducePoints(1);
                    //     addedPoints = false;
                    // }
                // }
                inputChanged = false;
                return;
            }
        }

        inputChanged = false;
    }
}