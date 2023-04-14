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
    private string l_childConsole, r_childConsole;
    private BlockDrag _l_blockDrop, _r_blockDrop;

    public override void Start() {
        base.Start();

        operation.onValueChanged.AddListener(new UnityAction<int>(index => OnInputFieldValueChanged(operation.options[index].text)));
    }

    public override void Update() {
        base.Update();

        if (l_dropBlock.transform.childCount > 0) {
            _l_blockDrop = l_dropBlock.transform.GetChild(0).GetComponent<BlockDrag>();
        }
        if (r_dropBlock.transform.childCount > 0) {
            _r_blockDrop = r_dropBlock.transform.GetChild(0).GetComponent<BlockDrag>();
        }

        if (_l_blockDrop != null) {
            if (_l_blockDrop.consoleValue != l_childConsole) {
                l_childConsole = _l_blockDrop.consoleValue;
                inputChanged = true;
            }
        }

        if (_r_blockDrop != null) {
            if (_r_blockDrop.consoleValue != r_childConsole) {
                r_childConsole = _r_blockDrop.consoleValue;
                inputChanged = true;
            }
        }
    }

    private bool ValidateInput()
    {
        int operationValue = operation.value;
        if (operation.options[operationValue].text == operationAnswer)  {
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
        l_blockDropScript = l_dropBlock.transform.childCount > 0 ? l_dropBlock.transform.GetChild(0).GetComponent<BlockDrag>() : null;
        r_blockDropScript = r_dropBlock.transform.childCount > 0 ? r_dropBlock.transform.GetChild(0).GetComponent<BlockDrag>() : null;
    }

    public override void BlockValidation()
    {
        if (_dropZone == null || !inputChanged) return; // Don't check the validation when not on the drop block
        CheckDropBlockValue();

        int operationValue = operation.value;
        int l_value, r_value;

        if (operation.options[operationValue].text == "+") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int sum = l_value + r_value;
                consoleValue = sum.ToString();
            }
        }
        else if (operation.options[operationValue].text == "-") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            
            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int difference = l_value - r_value;
                consoleValue = difference.ToString();
            }
        }
        else if (operation.options[operationValue].text == "*") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            
            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int product = l_value * r_value;
                consoleValue = product.ToString();
            }
        }
        else if (operation.options[operationValue].text == "/") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int quotient = l_value / r_value;
                consoleValue = quotient.ToString();
            }
        }
        else if (operation.options[operationValue].text == "%") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int remainder = l_value % r_value;
                consoleValue = remainder.ToString();
            }
        }
        else if (operation.options[operationValue].text == "++") {
            if (l_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(false);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value)) {
                int increment = l_value++;
                consoleValue = increment.ToString();
            }
        }
        else if (operation.options[operationValue].text == "--") {
            if (l_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(false);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value)) {
                int decrement = l_value--;
                consoleValue = decrement.ToString();
            }
        }
        else if (operation.options[operationValue].text == "=") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            l_blockDropScript.consoleValue = r_blockDropScript.consoleValue;
        }
        else if (operation.options[operationValue].text == "+=") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            
            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int sum = l_value + r_value;
                consoleValue = sum.ToString();
            }
        }
        else if (operation.options[operationValue].text == "-=") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            
            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int diff = l_value - r_value;
                consoleValue = diff.ToString();
            }
        }
        else if (operation.options[operationValue].text == "*=") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int product = l_value * r_value;
                consoleValue = product.ToString();
            }
        }
        else if (operation.options[operationValue].text == "/=") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int quotient = l_value / r_value;
                consoleValue = quotient.ToString();
            }
        }
        else if (operation.options[operationValue].text == "%=") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int remainder = l_value % r_value;
                consoleValue = remainder.ToString();
            }
        }
        else if (operation.options[operationValue].text == "&&") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (l_blockDropScript.consoleValue == "true" && r_blockDropScript.consoleValue == "true") {
                consoleValue = "true";
            }
            else {
                consoleValue = "false";
            }
        }
        else if (operation.options[operationValue].text == "||") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (l_blockDropScript.consoleValue == "true" || r_blockDropScript.consoleValue == "true") {
                consoleValue = "true";
            }
            else {
                consoleValue = "false";
            }
        }
        else if (operation.options[operationValue].text == "!") {
            if (r_blockDropScript == null) return;
            l_dropBlock.SetActive(false);
            r_dropBlock.SetActive(true);

            if (r_blockDropScript.consoleValue == "true") {
                consoleValue = "false";
            }
            else {
                consoleValue = "true";
            }
        }
        else if (operation.options[operationValue].text == "==") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (l_blockDropScript.consoleValue == r_blockDropScript.consoleValue) {
                consoleValue = "true";
            }
            else {
                consoleValue = "false";
            }
        }
        else if (operation.options[operationValue].text == "!=") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (l_blockDropScript.consoleValue != r_blockDropScript.consoleValue) {
                consoleValue = "true";
            }
            else {
                consoleValue = "false";
            }
        }
        else if (operation.options[operationValue].text == ">") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                if (l_value > r_value) {
                    consoleValue = "true";
                }
                else {
                    consoleValue = "false";
                }
            }
        }
        else if (operation.options[operationValue].text == "<") {
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

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
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

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
            if (l_blockDropScript == null || r_blockDropScript == null) return;
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                if (l_value >= r_value) {
                    consoleValue = "true";
                }
                else {
                    consoleValue = "false";
                }
            }
        }

        RefreshContentFitter((RectTransform)_environmentParent);

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                error = false;
                // if (addConsoleValue) consoleValue = inputField.text;

                // if (ValidateInput())
                // {
                    if (!addedPoints && addPoints)
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
                return;
            }
            else {
                error = true;
            }
        }

        inputChanged = false;
    }
}