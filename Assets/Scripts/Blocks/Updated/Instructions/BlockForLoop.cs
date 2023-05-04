using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Linq;
using Block;

public class BlockForLoop : BlockDrag
{
    [Header ("Objects")]
    [SerializeField] private GameObject childContainer;
    [SerializeField] private GameObject conditionValue;
    [SerializeField] private TMP_InputField variableValue;
    [SerializeField] private TMP_Dropdown conditionOperator;
    [SerializeField] private TMP_Dropdown assignOperator;
    [SerializeField] private BlockVariable variable;

    [Header ("Settings")]
    [SerializeField] private int maxLoop = 100;
    [SerializeField] private int ctr = 0;
    [Header ("Answers")]
    [SerializeField] private int loopCountAnswer;
    [SerializeField] private string conditionOperatorAnswer, incrementOperatorAnswer, varValueAnswer;
    private int childCount;
    private BlockDrag blockDrag = null;

    public override void Start() {
        base.Start();
        variableValue.onValueChanged.AddListener(OnInputFieldValueChanged);
        conditionOperator.onValueChanged.AddListener(new UnityAction<int>(index => OnInputFieldValueChanged(conditionOperator.options[index].text)));
        assignOperator.onValueChanged.AddListener(new UnityAction<int>(index => OnInputFieldValueChanged(assignOperator.options[index].text)));
    }

    public override void Update() {
        base.Update();
        
        if (conditionValue.transform.childCount != childCount) {
            childCount = conditionValue.transform.childCount;
            inputChanged = true;
        }
    }

    private void OnInputFieldValueChanged(string newValue)
    {
        inputChanged = true;
    }

    private bool ValidateInput() {
        string conditionOperatorText = conditionOperator.options[conditionOperator.value].text;
        string incrementOperatorText = assignOperator.options[assignOperator.value].text;
        if (conditionOperatorText == conditionOperatorAnswer 
            && incrementOperatorText == incrementOperatorAnswer
            && varValueAnswer == variableValue.text) {
                return true;
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
        int varValue = 0;
        ctr = 0;
        int operationValue = conditionOperator.value;
        int incrementValue = assignOperator.value;

        if (int.TryParse(variableValue.text, out varValue)) {
            if (conditionValue.transform.childCount > 0) {
                BlockDrag _conditionBlock = conditionValue.transform.GetChild(0).GetComponent<BlockDrag>();

                if (_conditionBlock != null) {
                    int conditionValue = 0;
                    if (int.TryParse(_conditionBlock.consoleValue, out conditionValue)) {
                        string conditionOperatorText = conditionOperator.options[operationValue].text;
                        string incrementOperatorText = assignOperator.options[incrementValue].text;
                        variable.SetDictionaryValue(varValue.ToString());
                        int loopVar = variable._intVar.Values.First();

                        switch (conditionOperatorText) {
                            case ">":
                                if (incrementOperatorText == "++") {
                                    for (; loopVar > conditionValue; loopVar++) {
                                        if (ctr > maxLoop || validationManager.errorDetected) break;
                                        variable.SetDictionaryValue(loopVar.ToString());
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                                        ctr++;
                                    }
                                } else if (incrementOperatorText == "--") {
                                    for (; loopVar > conditionValue; loopVar--) {
                                        if (ctr > maxLoop || validationManager.errorDetected) break;
                                        variable.SetDictionaryValue(loopVar.ToString());
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                                        ctr++;
                                    }
                                }
                                inputChanged = true;
                                break;
                            case "<":
                                if (incrementOperatorText == "++") {
                                    for (; loopVar < conditionValue; loopVar++) {
                                        if (ctr > maxLoop || validationManager.errorDetected) break;
                                        variable.SetDictionaryValue(loopVar.ToString());
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                                        ctr++;
                                    }
                                } else if (incrementOperatorText == "--") {
                                    for (; loopVar < conditionValue; loopVar--) {
                                        if (ctr > maxLoop || validationManager.errorDetected) break;
                                        variable.SetDictionaryValue(loopVar.ToString());
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                                        ctr++;
                                    }
                                }
                                inputChanged = true;
                                break;
                            case ">=":
                                if (incrementOperatorText == "++") {
                                    for (; loopVar >= conditionValue; loopVar++) {
                                        if (ctr > maxLoop || validationManager.errorDetected) break;
                                        variable.SetDictionaryValue(loopVar.ToString());
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                                        ctr++;
                                    }
                                } else if (incrementOperatorText == "--") {
                                    for (; loopVar >= conditionValue; loopVar--) {
                                        if (ctr > maxLoop || validationManager.errorDetected) break;
                                        variable.SetDictionaryValue(loopVar.ToString());
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                                        ctr++;
                                    }
                                }
                                inputChanged = true;
                                break;
                            case "<=":
                                if (incrementOperatorText == "++") {
                                    for (; loopVar <= conditionValue; loopVar++) {
                                        if (ctr > maxLoop || validationManager.errorDetected) break;
                                        variable.SetDictionaryValue(loopVar.ToString());
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                                        ctr++;
                                    }
                                } else if (incrementOperatorText == "--") {
                                    for (; loopVar <= conditionValue; loopVar--) {
                                        if (ctr > maxLoop || validationManager.errorDetected) break;
                                        variable.SetDictionaryValue(loopVar.ToString());
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                                        ctr++;
                                    }
                                }
                                inputChanged = true;
                                break;
                            case "==":
                                if (incrementOperatorText == "++") {
                                    for (; loopVar == conditionValue; loopVar++) {
                                        if (ctr > maxLoop || validationManager.errorDetected) break;
                                        variable.SetDictionaryValue(loopVar.ToString());
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                                        ctr++;
                                    }
                                } else if (incrementOperatorText == "--") {
                                    for (; loopVar == conditionValue; loopVar--) {
                                        if (ctr > maxLoop || validationManager.errorDetected) break;
                                        variable.SetDictionaryValue(loopVar.ToString());
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                                        ctr++;
                                    }
                                }
                                inputChanged = true;
                                break;
                            case "!=":
                                if (incrementOperatorText == "++") {
                                    for (; loopVar != conditionValue; loopVar++) {
                                        if (ctr > maxLoop || validationManager.errorDetected) break;
                                        variable.SetDictionaryValue(loopVar.ToString());
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                                        ctr++;
                                    }
                                } else if (incrementOperatorText == "--") {
                                    for (; loopVar != conditionValue; loopVar--) {
                                        if (ctr > maxLoop || validationManager.errorDetected) break;
                                        variable.SetDictionaryValue(loopVar.ToString());
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(UpdateBlocks(childContainer.transform));
                                        yield return new WaitForSeconds(0.1f);
                                        yield return StartCoroutine(LoopChildBlocks(childContainer.transform));
                                        ctr++;
                                    }
                                }
                                inputChanged = true;
                                break;
                            }
                            
                    }
                }
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