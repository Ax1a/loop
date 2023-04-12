using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BlockInput : BlockDrag
{
    public TMP_InputField inputField;
    public string answer;

    [Header ("For Printing to Console Log")]
    public string consoleValue;
    [SerializeField] private bool addConsoleValue;
    [SerializeField] private string currentAnswer;

    public override void Start() {
        base.Start();

        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    private bool ValidateInput()
    {
        if (!inputField.multiLine) {
            return inputField.text.ToLower() == answer.ToLower();
        }
        else if (inputField.multiLine) {
            if (inputField.text.Contains(answer)) return true;
        }

        return inputField.text.ToLower() == answer.ToLower();
    }

    // Function to check if input or dropdown changes
    // This prevents the block validation for always checking
    private void OnInputFieldValueChanged(string newValue)
    {
        inputChanged = true;
    }

    public override void BlockValidation()
    {
        currentAnswer = inputField.text;
        if (_dropZone == null || !inputChanged) return; // Don't check the validation when not on the drop block

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                if (addConsoleValue) consoleValue = inputField.text;

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
            }
        }

        inputChanged = false;
    }
}