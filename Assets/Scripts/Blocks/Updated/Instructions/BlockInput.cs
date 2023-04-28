using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BlockInput : BlockDrag
{
    [Header ("Objects")]
    public TMP_InputField inputField;
    [Header ("Answer")]
    public string answer;
    private Coroutine typingCoroutine;

    public override void Start() {
        base.Start();
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    private bool ValidateInput()
    {
        if (!inputField.multiLine) {
            if (!string.IsNullOrEmpty(answer)) {
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

            if (lines.Length == answerLines.Length) {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Trim() != answerLines[i].Trim())
                    {
                        allLinesMatch = false;
                        break;
                    }
                }
            }
            else {
                allLinesMatch = false;
            }
            return allLinesMatch;
        }
    }

    // Function to check if input or dropdown changes
    // This prevents the block validation for always checking
    private void OnInputFieldValueChanged(string newValue)
    {
        inputChanged = true;

        // Cancel any previous typing coroutine
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypingCoroutine());
    }

    private IEnumerator TypingCoroutine()
    {
        // Wait for the typing delay
        yield return new WaitForSeconds(1);
        Debug.Log("executed");
        // Execute your function here, passing the input field value as a parameter
        RefreshContentFitter((RectTransform)_environmentParent);

        typingCoroutine = null;
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
                inputChanged = false;
                return;
            }
            else {
                error = true;
            }
        }

        inputChanged = false;
    }
}