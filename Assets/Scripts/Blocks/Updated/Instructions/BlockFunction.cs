using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BlockFunction :  BlockDrag
{
    public TMP_InputField[] inputField;
    public TMP_Dropdown[] typesDropdown;
    public string[] inputAnswers;
    public string[] dropdownAnswers;

    public override void Start() {
        base.Start();

        foreach (var input in inputField)
        {
            input.onValueChanged.AddListener(OnInputFieldValueChanged);
        }

        foreach (var dropdown in typesDropdown)
        {
            dropdown.onValueChanged.AddListener(new UnityAction<int>(index => OnInputFieldValueChanged(dropdown.options[index].text)));
        }
    }

    private bool ValidateInput()
    {
        if (blockLanguage == BlockLanguage.Java) return true;
        for (int i = 0; i < inputField.Length; i++)
        {
            if (inputField[i].text.ToLower() == inputAnswers[i].ToLower()) {
                if (inputAnswers[i].ToLower() == "main") error = false;
                return true;
            }
            else {
                if (inputAnswers[i].ToLower() == "main") error = true;
            }
        }
        return false;
    }

    private bool ValidateDrowdown() {
        if (blockLanguage == BlockLanguage.Java) return true;
        for (int i = 0; i < typesDropdown.Length; i++)
        {
            int value = typesDropdown[i].value;
            if (typesDropdown[i].options[value].text.ToLower() == dropdownAnswers[i].ToLower()) {
                return true;
            }
        }
        return false;
    }

    // Function to check if input or dropdown changes
    // This prevents the block validation for always checking
    private void OnInputFieldValueChanged(string newValue)
    {
        inputChanged = true;
    }

    public override void BlockValidation()
    {
        if (_dropZone == null || !inputChanged) return; // Don't check the validation when not on the drop block
        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                error = false;
                if (ValidateInput() && ValidateDrowdown())
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