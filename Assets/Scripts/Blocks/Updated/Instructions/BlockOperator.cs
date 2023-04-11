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
    // public string answer;


    public override void Start() {
        base.Start();

        operation.onValueChanged.AddListener(new UnityAction<int>(index => OnInputFieldValueChanged(operation.options[index].text)));
    }

    // private bool ValidateInput()
    // {
    //     return inputField.text.ToLower() == answer.ToLower();
    // }

    // // Function to check if input or dropdown changes
    // // This prevents the block validation for always checking
    private void OnInputFieldValueChanged(string newValue)
    {
        inputChanged = true;
    }

    public override void BlockValidation()
    {
        if (_dropZone == null || !inputChanged) return; // Don't check the validation when not on the drop block

        int operationValue = operation.value;

        if (operation.options[operationValue].text == "+") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "-") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "*") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "/") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
        }
        else if (operation.options[operationValue].text == "%") {
            l_dropBlock.SetActive(true);
            r_dropBlock.SetActive(true);
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