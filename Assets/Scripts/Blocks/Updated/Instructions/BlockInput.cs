using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BlockInput : BlockDrag
{
    public TMP_InputField inputField;
    public string answer;

    public override void Start() {
        base.Start();
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    private bool ValidateInput()
    {
        if (!inputField.multiLine) {
            if (answer != "") {
                return inputField.text.ToLower() == answer.ToLower();
            }
            else {
                return true;
            }
        }
        else {
            string[] lines = inputField.text.Split('\n'); // Split the input text into an array of lines
            string[] answerLines = answer.Replace("\\n", "\n").Split(new string[] { "\n" }, StringSplitOptions.None);
            bool allLinesMatch = true;

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim() != answerLines[i].Trim())
                {
                    allLinesMatch = false;
                    break;
                }
            }
            return allLinesMatch;
        }
    }

    // Function to check if input or dropdown changes
    // This prevents the block validation for always checking
    private void OnInputFieldValueChanged(string newValue)
    {
        inputChanged = true;
    }

    public override void BlockValidation()
    {
        if (gameObject.name.StartsWith("C_Include") || gameObject.name.StartsWith("C_Using")) {
            error = inputField.text.ToLower() != answer.ToLower();
            if (addedPoints && addPoints && error)
            {
                validationManager.ReducePoints(1);
                addedPoints = false;
            }

            if (error) return;
        }

        if (_dropZone == null || !inputChanged) return; // Don't check the validation when not on the drop block

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                error = false;
                consoleValue = inputField.text;

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

                return;
            }
            else {
                error = true;
            }
        }

        inputChanged = false;
    }
}