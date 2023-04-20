using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class BlockOperator : BlockDrag
{
    [SerializeField] private GameObject l_dropBlock;
    [SerializeField] private GameObject r_dropBlock;
    [SerializeField] private TMP_Dropdown operation;
    private BlockDrag l_blockDropScript, r_blockDropScript;
    [SerializeField] private string operationAnswer;
    [SerializeField] private string l_blockAnswer;
    [SerializeField] private string r_blockAnswer;
    public string l_childConsole, r_childConsole;

    public override void Start() {
        base.Start();

        operation.onValueChanged.AddListener(new UnityAction<int>(index => OnInputFieldValueChanged(operation.options[index].text)));
    }

    public override void Update() {
        base.Update();

        if (l_blockDropScript != null) {
            if (l_blockDropScript.consoleValue != l_childConsole) {
                BlockVariable blockVariable = null;
                l_childConsole = l_blockDropScript.consoleValue;
                
                if (l_dropBlock?.transform.GetChild(0)?.transform.GetChild(0)?.childCount > 0) {
                    blockVariable = l_dropBlock.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockVariable>();
                }
                if (blockVariable == null){
                    inputChanged = true;
                }
            }
        }

        if (r_blockDropScript != null) {
            if (r_blockDropScript.consoleValue != r_childConsole) {
                r_childConsole = r_blockDropScript.consoleValue;
                inputChanged = true;
            }
        }
    }

    private bool ValidateInput()
    {
        int operationValue = operation.value;
        if (operation.options[operationValue].text == operationAnswer)  {
            if (l_blockDropScript != null && r_blockDropScript != null){
                Debug.Log(l_blockDropScript.consoleValue == l_blockAnswer && r_blockDropScript.consoleValue == r_blockAnswer);
                if (l_blockAnswer != "" && r_blockAnswer == "") {
                    return l_blockDropScript.consoleValue == l_blockAnswer;
                }
                else if (r_blockAnswer != "" && l_blockAnswer == "") {
                    return r_blockDropScript.consoleValue == r_blockAnswer;
                }
                else {
                    return l_blockDropScript.consoleValue == l_blockAnswer && r_blockDropScript.consoleValue == r_blockAnswer;
                }
            }
            else if (l_blockDropScript != null) {
                return l_blockDropScript.consoleValue == l_blockAnswer;
            }
            else if (r_blockDropScript != null) {
                return r_blockDropScript.consoleValue == r_blockAnswer;
            }
            
            return true;
        }

        return false;
    }

    // // Function to check if input or dropdown changes
    // // This prevents the block validation for always checking
    private void OnInputFieldValueChanged(string newValue)
    {
        inputChanged = true;
    }

    public void CheckDropBlockValue() {
        if (l_dropBlock != null) {
            l_blockDropScript = l_dropBlock.transform.childCount > 0 ? l_dropBlock.transform.GetChild(0).GetComponent<BlockDrag>() : null;
        }
        else {
            l_blockDropScript = null;
        }
        if (r_dropBlock != null) {
            r_blockDropScript = r_dropBlock.transform.childCount > 0 ? r_dropBlock.transform.GetChild(0).GetComponent<BlockDrag>() : null;
        }
        else {
            r_blockDropScript = null;
        }
    }

    public override void BlockValidation()
    {
        CheckDropBlockValue();
        if (_dropZone == null || !inputChanged) return; // Don't check the validation when not on the drop block

        RefreshContentFitter((RectTransform)_environmentParent);

        ExecuteOperator();

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                error = false;
                // if (addConsoleValue) consoleValue = inputField.text;

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

    public void IncrementValue() {
        int operationValue = operation.value;
        int l_value;
        if (operation.options[operationValue].text == "++") {
            l_dropBlock.SetActive(true);
            if (l_blockDropScript == null) return;

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value)) {
                int increment = ++l_value;

                if (l_dropBlock?.transform.GetChild(0)?.transform.GetChild(0)?.childCount > 0) {
                    // int value;
                    BlockVariable blockVariable = l_dropBlock.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockVariable>();
                    if ( blockVariable != null) {
                        // value++;
                        blockVariable.consoleValue = increment.ToString();
                        blockVariable.SetDictionaryValue(increment.ToString());
                    }
                }
                consoleValue = increment.ToString();
            }

        }
        else if (operation.options[operationValue].text == "--") {
            l_dropBlock.SetActive(true);
            if (l_blockDropScript == null) return;

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value)) {
                int decrement = --l_value;
                // consoleValue = decrement.ToString();

                if (l_dropBlock?.transform.GetChild(0)?.transform.GetChild(0)?.childCount > 0) {
                    // int value;
                    BlockVariable blockVariable = l_dropBlock.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockVariable>();
                    if (blockVariable != null) {
                        // value--;
                        blockVariable.consoleValue = decrement.ToString();
                        blockVariable.SetDictionaryValue(decrement.ToString());
                    }
                }
                consoleValue = decrement.ToString();
            }
        }
    }

    public void ExecuteOperator() {
        int operationValue = operation.value;
        int l_value, r_value;

        if (operation.options[operationValue].text == "+") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int sum = l_value + r_value;
                consoleValue = sum.ToString();
            }
            else {
                string concat = l_blockDropScript.consoleValue + r_blockDropScript.consoleValue;
                
                if (l_dropBlock?.transform.GetChild(0)?.transform.GetChild(0)?.childCount > 0) {
                    BlockVariable blockVariable = l_dropBlock.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockVariable>();
                    if (blockVariable != null) {
                        blockVariable.SetDictionaryValue(concat);
                    }
                }
            }
        }
        else if (operation.options[operationValue].text == "-") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            
            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int difference = l_value - r_value;
                consoleValue = difference.ToString();
            }
        }
        else if (operation.options[operationValue].text == "*") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            
            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int product = l_value * r_value;
                consoleValue = product.ToString();
            }
        }
        else if (operation.options[operationValue].text == "/") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int quotient = l_value / r_value;
                consoleValue = quotient.ToString();
            }
        }
        else if (operation.options[operationValue].text == "%") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int remainder = l_value % r_value;
                consoleValue = remainder.ToString();
            }
        }
        else if (operation.options[operationValue].text == "=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            l_blockDropScript.consoleValue = r_blockDropScript.consoleValue;
            consoleValue = r_blockDropScript.consoleValue;

            if (l_dropBlock?.transform.GetChild(0)?.transform.GetChild(0)?.childCount > 0) {
                BlockVariable blockVariable = l_dropBlock.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockVariable>();
                if (blockVariable != null) {
                    blockVariable.SetDictionaryValue(r_blockDropScript.consoleValue);
                }
            }
        }
        else if (operation.options[operationValue].text == "+=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            
            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int sum = l_value + r_value;

                if (l_dropBlock?.transform.GetChild(0)?.transform.GetChild(0)?.childCount > 0) {
                    BlockVariable blockVariable = l_dropBlock.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockVariable>();
                    if (blockVariable != null) {
                        blockVariable.SetDictionaryValue(sum.ToString());
                    }
                }
                consoleValue = sum.ToString();
            }
            else {
                string concat = l_blockDropScript.consoleValue + r_blockDropScript.consoleValue;
                
                if (l_dropBlock?.transform.GetChild(0)?.transform.GetChild(0)?.childCount > 0) {
                    BlockVariable blockVariable = l_dropBlock.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockVariable>();
                    if (blockVariable != null) {
                        blockVariable.SetDictionaryValue(concat);
                    }
                }
            }
        }
        else if (operation.options[operationValue].text == "-=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            
            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int diff = l_value - r_value;
                consoleValue = diff.ToString();
            }

            if (l_dropBlock?.transform.GetChild(0)?.transform.GetChild(0)?.childCount > 0) {
                BlockVariable blockVariable = l_dropBlock.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockVariable>();
                
                if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value) && blockVariable != null) {
                    int difference = l_value - r_value;
                     blockVariable.SetDictionaryValue(difference.ToString());
                }
            }
        }
        else if (operation.options[operationValue].text == "*=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int product = l_value * r_value;
                consoleValue = product.ToString();
            }

            if (l_dropBlock?.transform.GetChild(0)?.transform.GetChild(0)?.childCount > 0) {
                BlockVariable blockVariable = l_dropBlock.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockVariable>();
                
                if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value) && blockVariable != null) {
                    int prod = l_value * r_value;
                     blockVariable.SetDictionaryValue(prod.ToString());
                }
            }
        }
        else if (operation.options[operationValue].text == "/=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int quotient = l_value / r_value;
                consoleValue = quotient.ToString();
            }

            if (l_dropBlock?.transform.GetChild(0)?.transform.GetChild(0)?.childCount > 0) {
                BlockVariable blockVariable = l_dropBlock.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockVariable>();
                
                if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value) && blockVariable != null) {
                    int quote = l_value / r_value;
                     blockVariable.SetDictionaryValue(quote.ToString());
                }
            }
        }
        else if (operation.options[operationValue].text == "%=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int remainder = l_value % r_value;
                consoleValue = remainder.ToString();
            }

            if (l_dropBlock?.transform.GetChild(0)?.transform.GetChild(0)?.childCount > 0) {
                BlockVariable blockVariable = l_dropBlock.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<BlockVariable>();
                
                if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value) && blockVariable != null) {
                    int remainder = l_value % r_value;
                     blockVariable.SetDictionaryValue(remainder.ToString());
                }
            }
        }
        else if (operation.options[operationValue].text == "&&" || operation.options[operationValue].text == "and") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (l_blockDropScript.consoleValue == "true" && r_blockDropScript.consoleValue == "true") {
                consoleValue = "true";
            }
            else {
                consoleValue = "false";
            }
        }
        else if (operation.options[operationValue].text == "||" || operation.options[operationValue].text == "or") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (l_blockDropScript.consoleValue == "true" || r_blockDropScript.consoleValue == "true") {
                consoleValue = "true";
            }
            else {
                consoleValue = "false";
            }
        }
        else if (operation.options[operationValue].text == "!" || operation.options[operationValue].text == "not") {
            l_dropBlock.SetActive(false);
            r_dropBlock.SetActive(true);
            if (r_blockDropScript == null) return;

            if (r_blockDropScript.consoleValue == "true") {
                consoleValue = "false";
            }
            else {
                consoleValue = "true";
            }
        }
        else if (operation.options[operationValue].text == "==") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (l_blockDropScript.consoleValue == r_blockDropScript.consoleValue) {
                consoleValue = "true";
            }
            else {
                consoleValue = "false";
            }
        }
        else if (operation.options[operationValue].text == "!=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (l_blockDropScript.consoleValue != r_blockDropScript.consoleValue) {
                consoleValue = "true";
            }
            else {
                consoleValue = "false";
            }
        }
        else if (operation.options[operationValue].text == ">") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                Debug.Log(l_value > r_value);
                if (l_value > r_value) {
                    consoleValue = "true";
                }
                else {
                    consoleValue = "false";
                }
            }
        }
        else if (operation.options[operationValue].text == "<") {
            r_dropBlock.SetActive(true);
            l_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                if (l_value < r_value) {
                    consoleValue = "true";
                }
                else {
                    consoleValue = "false";
                }
            }
        }
        else if (operation.options[operationValue].text == ">=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                if (l_value >= r_value) {
                    consoleValue = "true";
                }
                else {
                    consoleValue = "false";
                }
            }
        }
        else if (operation.options[operationValue].text == "<=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            if (l_blockDropScript == null || r_blockDropScript == null) return;

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                if (l_value >= r_value) {
                    consoleValue = "true";
                }
                else {
                    consoleValue = "false";
                }
            }
        }
    }
}