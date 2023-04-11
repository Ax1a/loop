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
    private BlockOneDrop l_blockDropScript, r_blockDropScript;
    [SerializeField] private string operationAnswer;
    [SerializeField] private string l_blockAnswer;
    [SerializeField] private string r_blockAnswer;
    private int l_childCount, r_childCount;
    public string consoleValue;

    public override void Start() {
        base.Start();

        l_childCount = l_dropBlock.transform.childCount;
        r_childCount = r_dropBlock.transform.childCount;
        operation.onValueChanged.AddListener(new UnityAction<int>(index => OnInputFieldValueChanged(operation.options[index].text)));
    }

    public override void Update() {
        base.Update();

        if (l_dropBlock.transform.childCount != l_childCount) {
            l_childCount = l_dropBlock.transform.childCount;
            inputChanged = true;
        }
        if (r_dropBlock.transform.childCount != r_childCount) {
            r_childCount = r_dropBlock.transform.childCount;
            inputChanged = true;
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
        if (l_dropBlock.transform.childCount > 0 && r_dropBlock.transform.childCount > 0) {
            l_blockDropScript = l_dropBlock.transform.GetChild(0).GetComponent<BlockOneDrop>();
            r_blockDropScript = r_dropBlock.transform.GetChild(0).GetComponent<BlockOneDrop>();
        }
        else if (l_dropBlock.transform.childCount > 0) {
            l_blockDropScript = l_dropBlock.transform.GetChild(0).GetComponent<BlockOneDrop>();
        }
        else if (r_dropBlock.transform.childCount > 0) {
            r_blockDropScript = r_dropBlock.transform.GetChild(0).GetComponent<BlockOneDrop>();
        }
    }

    public override void BlockValidation()
    {
        if (_dropZone == null || !inputChanged) return; // Don't check the validation when not on the drop block

        int operationValue = operation.value;

        CheckDropBlockValue();
        int l_value, r_value;

        if (operation.options[operationValue].text == "+") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int sum = l_value + r_value;
                consoleValue = sum.ToString();
            }
        }
        else if (operation.options[operationValue].text == "-") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            
            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int difference = l_value - r_value;
                consoleValue = difference.ToString();
            }
        }
        else if (operation.options[operationValue].text == "*") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
            
            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int product = l_value * r_value;
                consoleValue = product.ToString();
            }
        }
        else if (operation.options[operationValue].text == "/") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int quotient = l_value / r_value;
                consoleValue = quotient.ToString();
            }
        }
        else if (operation.options[operationValue].text == "%") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);

            if (int.TryParse(l_blockDropScript.consoleValue, out l_value) && int.TryParse(r_blockDropScript.consoleValue, out r_value)) {
                int remainder = l_value % r_value;
                consoleValue = remainder.ToString();
            }
        }
        else if (operation.options[operationValue].text == "++") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(false);
        }
        else if (operation.options[operationValue].text == "--") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(false);
        }
        else if (operation.options[operationValue].text == "=") {            
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "+=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "-=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "*=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "/=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "%=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "&&") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "||") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "!") {
            l_dropBlock.SetActive(false);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "==") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "!=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == ">") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "<") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == ">=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "<=") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }

        RefreshContentFitter((RectTransform)_environmentParent);

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                // if (addConsoleValue) consoleValue = inputField.text;

                // if (ValidateInput())
                // {
                //     if (!addedPoints)
                //     {
                //         validationManager.AddPoints(1);
                //         addedPoints = true;
                //     }
                // }
                // else
                // {
                //     if (addedPoints)
                //     {
                //         validationManager.ReducePoints(1);
                //         addedPoints = false;
                //     }
                // }
            }
        }

        inputChanged = false;
    }
}