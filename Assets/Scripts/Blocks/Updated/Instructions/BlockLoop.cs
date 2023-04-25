using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLoop : BlockDrag
{
    [Header ("Objects")]
    public GameObject dropBlock;
    public GameObject childContainer;
    [Header ("Settings")]
    [SerializeField] private int maxLoop = 50;
    [SerializeField] private LoopType loopType;
    [Header ("Answers")]
    [SerializeField] private int loopCountAnswer;
    [SerializeField] private int ctr = 0;
    private enum LoopType { While, DoWhile }
    private int childCount;
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
                    BlockOneDrop blockOneDrop = child.GetComponent<BlockOneDrop>();

                    if (blockDragChild.error) {
                        validationManager.errorDetected = true;
                        break;
                    }

                    if (child.name.StartsWith("C_IfCondition") && !blockDragChild.consoleValue.ToLower().Equals("true") ||
                        child.name.StartsWith("J_IfCondition") && !blockDragChild.consoleValue.ToLower().Equals("true") ||
                        child.name.StartsWith("P_IfCondition") && !blockDragChild.consoleValue.ToLower().Equals("true"))
                    {
                        continue;
                    }

                    if (blockOperator != null) {
                        StartCoroutine(blockOperator.IncrementValue());
                    }

                    // if (blockOneDrop != null && child.transform.name.StartsWith("C_CharInput")) {
                    //     BlockVariable _blockVariable = blockOneDrop.dropBlock.transform.GetChild(0).GetComponent<BlockVariable>();
                    //     BlockDrag _blockDrag = blockOneDrop.dropBlock.transform.GetChild(0).GetComponent<BlockDrag>();

                    //     if (_blockVariable != null) {
                    //         validationManager.AskForInput(_blockVariable);
                    //         processWait = true;
                    //         yield return WaitForProcessToFinish();
                    //     }
                    //     else if (_blockDrag != null && _blockDrag.consoleValue.Length != 0 && _blockDrag.printConsole) {
                    //         _blockDrag.consoleValue = "";
                    //     }
                    // }
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
            while (consoleValue == "true" && ctr < maxLoop && !validationManager.errorDetected) {
                yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                yield return new WaitForSeconds(0.1f);
                yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                yield return new WaitForSeconds(0.1f);
                ctr++;
            }
        }
        else if (loopType == LoopType.DoWhile) {
            do {
                yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                yield return new WaitForSeconds(0.1f);
                yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                yield return new WaitForSeconds(0.1f);
                ctr++;
            }
            while (consoleValue == "true" && ctr < maxLoop && !validationManager.errorDetected);
        }

        // Remove the last line from the console log
        
        inputChanged = true;
        StartCoroutine(validationManager.DelayDisable());
    }

    private void CheckHeaderOperator() {
        if (dropBlock.transform.childCount > 0) {
            blockDrag = dropBlock.transform.GetChild(0)?.GetComponent<BlockDrag>();

            if (blockDrag.consoleValue == "true") {
                consoleValue = "true";
            }
            else {
                consoleValue = "false";
            }
        }
        else {
            error = true;
        }
    }

    public override void BlockValidation()
    {
        if (_dropZone == null || instantiate || !inputChanged) return; // Don't check the validation when not on the drop block
        if (dropBlock.transform.childCount == 0) return;

        CheckHeaderOperator();

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                if (!addedPoints && loopCountAnswer == ctr)
                {
                    validationManager.AddPoints(1);
                    addedPoints = true;
                }
                else if (addedPoints && loopCountAnswer != ctr)
                {
                    validationManager.ReducePoints(1);
                    addedPoints = false;
                }
                inputChanged = false;
                return;
            }
        }

        inputChanged = false;
    }
}