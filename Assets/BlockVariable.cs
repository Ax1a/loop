using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System.Linq;
using TMPro;

public class BlockVariable : BlockDrag
{
    public string answer;
    [Header ("Variable Attribute | One Only")]
    [SerializeField] private IntVariableArr _intArray;
    [SerializeField] private StringVariableArr _stringArray;
    [SerializeField] private StringVariable _stringVar;
    [SerializeField] private IntVariable _intVar;

    [Header ("Objects")]
    [SerializeField] private TextMeshProUGUI variableNameTxt;
    [SerializeField] private GameObject variableArray;

    [Header ("For Printing to Console Log")]
    public string consoleValue;
    [SerializeField] private bool addConsoleValue;

    public override void Start() {
        base.Start();

        // inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        if (_intArray.Count() > 0) {
            variableNameTxt.text = _intArray.Keys.First().ToString();
            variableArray.SetActive(true);
            foreach (var item in _intArray.First().Value.intArr)
            {
                Debug.Log(item);
            }
        }
        else if (_stringArray.Count() > 0) {
            variableNameTxt.text = _stringArray.Keys.First().ToString();
            variableArray.SetActive(true);
            foreach (var item in _stringArray.First().Value.stringArr)
            {
                Debug.Log(item);
            }
        }
        else if (_stringVar.Count() > 0) {
            variableArray.SetActive(false);
            variableNameTxt.text = _stringVar.Keys.First().ToString();
        }
        else if (_intVar.Count() > 0) {
            variableArray.SetActive(false);
            variableNameTxt.text = _intVar.Keys.First().ToString();
        }
    }

    // private bool ValidateInput()
    // {
    //     return inputField.text.ToLower() == answer.ToLower();
    // }

    // // Function to check if input or dropdown changes
    // // This prevents the block validation for always checking
    // private void OnInputFieldValueChanged(string newValue)
    // {
    //     inputChanged = true;
    // }

    public override void BlockValidation()
    {
        if (_dropZone == null || !inputChanged) return; // Don't check the validation when not on the drop block

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
        //     if (dropID == id)
        //     {
        //         if (addConsoleValue) consoleValue = inputField.text;

        //         if (ValidateInput())
        //         {
        //             if (!addedPoints)
        //             {
        //                 validationManager.AddPoints(1);
        //                 addedPoints = true;
        //             }
        //         }
        //         else
        //         {
        //             if (addedPoints)
        //             {
        //                 validationManager.ReducePoints(1);
        //                 addedPoints = false;
        //             }
        //         }
        //     }
        }

        inputChanged = false;
    }
}

// Variable Name, Variable Type
// <string,       variable>
[System.Serializable]
public class IntVariableArr : SerializableDictionaryBase<string, IntValuesArr> { }

[System.Serializable]
public class IntValuesArr
{
    public int[] intArr;
}

[System.Serializable]
public class StringVariableArr : SerializableDictionaryBase<string, StringValuesArr> { }

[System.Serializable]
public class StringValuesArr
{
    public string[] stringArr;
}

[System.Serializable]
public class StringVariable : SerializableDictionaryBase<string, string> { }

[System.Serializable]
public class IntVariable : SerializableDictionaryBase<string, int> { }
