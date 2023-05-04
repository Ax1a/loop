using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Linq;
using RotaryHeart.Lib.SerializableDictionary;
using Block;

public class BlockForLoopP : BlockDrag
{
    [Header ("Objects")]
    [SerializeField] private GameObject childContainer;
    [SerializeField] private GameObject sequenceValue;
    [SerializeField] private TMP_InputField variableName;
    [SerializeField] private BlockVariable loopVariable;

    [Header ("Settings")]
    [SerializeField] private int maxLoop = 200;
    [SerializeField] private int ctr = 0;
    [Header ("Answers")]
    [SerializeField] private int loopCountAnswer;
    [SerializeField] private GameObject sequenceAnswer;
    private int childCount;
    private BlockDrag blockDrag = null;
    private BlockVariable blockVariable = null;

    public override void Start() {
        base.Start();
        variableName.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    public override void Update() {
        base.Update();
        
        if (sequenceAnswer != null) {
            if (sequenceAnswer.transform.childCount != childCount) {
                childCount = sequenceAnswer.transform.childCount;
                inputChanged = true;
            }
        }

        if (sequenceValue != null) {
            if (sequenceValue.transform.childCount == 0) return;
            if (sequenceValue.transform.gameObject.transform.childCount == 0) return;

            if (sequenceValue?.transform.GetChild(0)?.transform.GetChild(0)?.childCount > 0) {
                blockVariable = sequenceValue.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockVariable>();
                
                if (blockVariable == null) {
                    blockDrag = sequenceValue.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockDrag>();
                    // foreach (var item in blockDrag.consoleValue)
                    // {
                    //     Debug.Log(item + "\n");
                    // }
                }
                else {
                    blockVariable.transform.GetChild(2).gameObject.SetActive(false);
                    RefreshContentFitter((RectTransform)_environmentParent);
                    
                }
            }
        }
    }

    private void OnInputFieldValueChanged(string newValue)
    {
        if (loopVariable != null) {
            if (loopVariable._intArray.Count > 0) {
                UpdateKey(loopVariable._intArray, loopVariable._intArray.Keys.First(), variableName.text);
            }
            else if (loopVariable._intVar.Count > 0) {
                UpdateKey(loopVariable._intVar, loopVariable._intVar.Keys.First(), variableName.text);
            }
            else if (loopVariable._stringArray.Count > 0) {
                UpdateKey(loopVariable._stringArray, loopVariable._stringArray.Keys.First(), variableName.text);
            }
            else if (loopVariable._stringVar.Count > 0) {
                UpdateKey(loopVariable._stringVar, loopVariable._stringVar.Keys.First(), variableName.text);
            }
        }

        inputChanged = true;
    }

    private void UpdateKey<TKey, TValue>(IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey)
    {
        TValue value = dic[fromKey];
        dic.Remove(fromKey);
        dic[toKey] = value;
    }

    private bool ValidateInput() {
        if (sequenceAnswer != null && sequenceValue != null) {
            if (sequenceValue == sequenceAnswer && loopCountAnswer == ctr) {
                return true;
            }
        }
        return false;
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
                    if (child.parent.GetComponent<BlockDrag>().error) {
                        validationManager.errorDetected = true;
                        break;
                    } 
                    if (child.parent.GetComponent<BlockDrag>()?.consoleValue == "false") continue;
                }
                else if (child.name.Equals("ElseCondition")) {
                    if (child.parent.GetComponent<BlockDrag>().error) {
                        validationManager.errorDetected = true;
                        break;
                    } 
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

                    if (blockDragChild.printConsole && blockDragChild.blockType == BlockType.Type1) {
                        validationManager._consoleLog += blockDragChild.consoleValue + "\n";
                    }
                }
                
                yield return LoopChildBlocks(child);
            }
        }
    }

    public IEnumerator DelayLoop() {
        if (blockVariable != null) {
            if (blockVariable._stringArray.Count > 0) {
                error = false;
                foreach (var item in blockVariable._stringArray.First().Value.stringArr)
                {
                    if (ctr > maxLoop || validationManager.errorDetected) break;
                    loopVariable.SetDictionaryValue(item.ToString());
                    yield return new WaitForSeconds(0.1f);
                    yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                    yield return new WaitForSeconds(0.1f);
                    yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                    ctr++;
                }
            }
            else if (blockVariable._intArray.Count > 0) {
                error = false;
                foreach (var item in blockVariable._intArray.First().Value.intArr)
                {
                    if (ctr > maxLoop || validationManager.errorDetected) break;
                    loopVariable.SetDictionaryValue(item.ToString());
                    yield return new WaitForSeconds(0.1f);
                    yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                    yield return new WaitForSeconds(0.1f);
                    yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                    ctr++;
                }
            }
            else if (blockVariable._intVar.Count > 0) {
                error = true;
            }
            else if (blockVariable._stringVar.Count > 0) {
                error = false;
                foreach (var item in blockVariable._stringVar.Values.First())
                {
                    if (ctr > maxLoop || validationManager.errorDetected) break;
                    loopVariable.SetDictionaryValue(item.ToString());
                    yield return new WaitForSeconds(0.1f);
                    yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                    yield return new WaitForSeconds(0.1f);
                    yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                    ctr++;
                }
            }
        }
        else {
            error = false;
            foreach (var item in blockDrag.consoleValue)
            {
                if (ctr > maxLoop || validationManager.errorDetected) break;
                loopVariable.SetDictionaryValue(item.ToString());
                yield return new WaitForSeconds(0.1f);
                yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                yield return new WaitForSeconds(0.1f);
                yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                ctr++;
            }
        }
        StartCoroutine(validationManager.DelayDisable());
    }

    public override void BlockValidation()
    {
        if (_dropZone == null || instantiate || !inputChanged) return; // Don't check the validation when not on the drop block

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                if (!addedPoints && loopCountAnswer == ctr && ValidateInput())
                {
                    validationManager.AddPoints(1);
                    addedPoints = true;
                }
                else if(addedPoints && !ValidateInput())
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