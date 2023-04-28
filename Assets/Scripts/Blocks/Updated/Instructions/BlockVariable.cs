using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System.Linq;
using TMPro;
using System;

public class BlockVariable : BlockDrag
{
    [SerializeField] private VarTypes varType;
    [Space (10)]
    public string answer;
    [Header ("Variable Attribute | One Only")]
    [SerializeField] public IntVariableArr _intArray;
    [SerializeField] public StringVariableArr _stringArray;
    [SerializeField] public StringVariable _stringVar;
    [SerializeField] public IntVariable _intVar;

    [Header ("Objects")]
    [SerializeField] private TextMeshProUGUI variableType;
    [SerializeField] private TextMeshProUGUI variableNameTxt;
    [SerializeField] private TMP_InputField variableArrayIndex;
    [SerializeField] private BlockVariable origBlockVariable = null;
    
    // Store the default value for variables
    [Header ("Ignore")]
    [SerializeField] private StringVariable _defStringVar;
    [SerializeField] private IntVariable _defIntVar;
    public bool declared = false, preDeclare = false, forLoopVar = false;
    private enum VarTypes { Int, Float, Double, String, Bool}

    public override void Start() {
        base.Start();
        if (forLoopVar) declared = true;
        SetVariableType();

        variableArrayIndex.onValueChanged.AddListener(OnInputFieldValueChanged);
        if (_intArray.Count() > 0) {
            variableNameTxt.text = _intArray.Keys.First().ToString();
            variableArrayIndex.transform.parent.gameObject.SetActive(true);
            if (blockLanguage == BlockLanguage.Python) variableArrayIndex.transform.parent.gameObject.SetActive(false);
        }
        else if (_stringArray.Count() > 0) {
            variableNameTxt.text = _stringArray.Keys.First().ToString();
            variableArrayIndex.transform.parent.gameObject.SetActive(true);
            if (blockLanguage == BlockLanguage.Python) variableArrayIndex.transform.parent.gameObject.SetActive(false);
        }
        else if (_stringVar.Count() > 0) {
            _defStringVar.Clear();
            foreach (var val in _stringVar) {
                _defStringVar.Add(val.Key, val.Value);
            }
            variableArrayIndex.transform.parent.gameObject.SetActive(false);
            variableNameTxt.text = _stringVar.Keys.First().ToString();
        }
        else if (_intVar.Count() > 0) {
            _defIntVar.Clear();
            foreach (var val in _intVar) {
                _defIntVar.Add(val.Key, val.Value);
            }
            variableArrayIndex.transform.parent.gameObject.SetActive(false);
            variableNameTxt.text = _intVar.Keys.First().ToString();
        }
        inputChanged = true;
        
    }

    private void SetVariableType() {
        // Set variable type
        if (varType == VarTypes.Int) {
            if (variableType != null) {
                if (blockLanguage == BlockLanguage.C) variableType.text = "int";
                if (blockLanguage == BlockLanguage.Java) variableType.text = "int";
            }
        }
        else if (varType == VarTypes.Float) {
            if (variableType != null) {
                if (blockLanguage == BlockLanguage.C) variableType.text = "float";
                if (blockLanguage == BlockLanguage.Java) variableType.text = "float";
            }
        }
        else if (varType == VarTypes.Double) {
            if (variableType != null) {
                if (blockLanguage == BlockLanguage.C) variableType.text = "double";
                if (blockLanguage == BlockLanguage.Java) variableType.text = "double";
            }
        }
        else if (varType == VarTypes.String) {
            if (variableType != null) {
                if (blockLanguage == BlockLanguage.C) variableType.text = "string";
                if (blockLanguage == BlockLanguage.Java) variableType.text = "String";
            }
        }
        else if (varType == VarTypes.Bool) {
            if (variableType != null) {
                if (blockLanguage == BlockLanguage.C) variableType.text = "bool";
                if (blockLanguage == BlockLanguage.Java) variableType.text = "boolean";
            }
        }
    }

    public void EnableVariableType() {
        if (variableType != null && (blockLanguage == BlockLanguage.C || blockLanguage == BlockLanguage.Java)) {
            variableType.gameObject.SetActive(true);
        }
        else {
            if (variableType != null) variableType.gameObject.SetActive(false);
        }

        variableArrayIndex.interactable = false;
        RefreshContentFitter((RectTransform)refreshParent);
    }


    public override void Update() {
        base.Update();

        if (validationManager.resetBlocks) {
            ResetVariables();
        }

        if (_intArray.Count() > 0) variableNameTxt.text = _intArray.Keys.First().ToString();
        if (_stringArray.Count() > 0) variableNameTxt.text = _stringArray.Keys.First().ToString();
        if (_stringVar.Count() > 0) variableNameTxt.text = _stringVar.Keys.First().ToString();
        if (_intVar.Count() > 0) variableNameTxt.text = _intVar.Keys.First().ToString();

        if (origBlockVariable != null && !instantiate) {
            declared = origBlockVariable.declared;
            
            if (_intArray.Count() > 0) {
                _intArray = origBlockVariable._intArray;

                if (variableArrayIndex.text != "") {
                    int index = int.Parse(variableArrayIndex.text);

                    if (index > _intArray.First().Value.intArr.Length - 1) {
                        consoleValue = "Array index out of bounds";
                    }
                    else {
                        consoleValue = _intArray.First().Value.intArr[index].ToString();
                    }
                }
            }
            else if (_stringArray.Count() > 0) {
                _stringArray = origBlockVariable._stringArray;
                
                if (variableArrayIndex.text != "") {
                    int index = int.Parse(variableArrayIndex.text);

                    if (index > _stringArray.First().Value.stringArr.Length - 1) {
                        consoleValue = "Array index out of bounds";
                    }
                    else {
                        consoleValue = _stringArray.First().Value.stringArr[index];
                    }
                }
            }
            else if (_stringVar.Count() > 0 && _stringVar != null) {
                _stringVar = origBlockVariable._stringVar;
                consoleValue = _stringVar.Values.First().ToString();
                
                originalObj.GetComponent<BlockDrag>().inputChanged = false;
                inputChanged = true;
            }
            else if (_intVar.Count() > 0 && _intVar != null) {
                _intVar = origBlockVariable._intVar;
                consoleValue = _intVar.Values.First().ToString();

                originalObj.GetComponent<BlockDrag>().inputChanged = false;
                inputChanged = true;
            }
        }
        else if (origBlockVariable == null && !instantiate) {
            if (originalObj != null) origBlockVariable = originalObj.GetComponent<BlockVariable>();
            inputChanged = true;
        }
    }

    // // Function to check if input or dropdown changes
    // // This prevents the block validation for always checking
    private void OnInputFieldValueChanged(string newValue)
    {
        inputChanged = true;
    }

    private bool ValidateInput()
    {
        string[] answerLines = answer.Split(new string[] { "|" }, StringSplitOptions.None);
        if (answerLines.Length > 1) {
            foreach (var answer in answerLines)
            {
                if (consoleValue.ToLower() == answer.ToLower()) return true;
            }

            return false;
        }

        if (!string.IsNullOrEmpty(answer)) {
            return consoleValue.ToLower() == answer.ToLower();
        }
        else {
            return true;
        }
    }

    public void AddNewElementToArray(string input) {
        if (_stringArray.Count > 0) {
            string[] lines = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => s.Trim().Trim('\"'))
                      .ToArray();

            for (int i = 0; i < lines.Length; i++)
            {
                // Get the StringValuesArr object associated with the key "myKey"
                StringValuesArr valuesArr = _stringArray[_stringArray.Keys.FirstOrDefault()];

                // Add a new element to the stringArr array
                Array.Resize(ref valuesArr.stringArr, valuesArr.stringArr.Length + 1);
                valuesArr.stringArr[valuesArr.stringArr.Length - 1] = lines[i];

                // Update the dictionary with the modified StringValuesArr object
                _stringArray[_stringArray.Keys.FirstOrDefault()] = valuesArr;
            }
        }
        else if (_intArray.Count > 0) {
            string[] lines = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => s.Trim().Trim('\"'))
                      .ToArray();

            for (int i = 0; i < lines.Length; i++)
            {
                // Get the StringValuesArr object associated with the key "myKey"
                IntValuesArr valuesArr = _intArray[_intArray.Keys.FirstOrDefault()];

                // Add a new element to the stringArr array
                Array.Resize(ref valuesArr.intArr, valuesArr.intArr.Length + 1);
                valuesArr.intArr[valuesArr.intArr.Length - 1] = Int32.Parse(lines[i]);

                // Update the dictionary with the modified StringValuesArr object
                _intArray[_intArray.Keys.FirstOrDefault()] = valuesArr;
            }
        }
    }

    public void SetDictionaryValue(string input) {
        if (_intArray.Count() > 0) {
            if (variableArrayIndex.text != "") {
                int index = int.Parse(variableArrayIndex.text);
                consoleValue = input;

                if (index > _intArray.First().Value.intArr.Length - 1) {
                    consoleValue = "Array index out of bounds";
                    // _intArray.First().Value.intArr[index] = 0;
                    // if (originalObj != null) originalObj.GetComponent<BlockVariable>()._intArray.First().Value.intArr[index] = 0;
                    error = true;
                }
                else {
                    error = false;
                    _intArray.First().Value.intArr[index] = int.Parse(input);
                    if (originalObj != null) originalObj.GetComponent<BlockVariable>()._intArray.First().Value.intArr[index] = int.Parse(input);
                }
            }
        }
        else if (_stringArray.Count() > 0) {
            if (variableArrayIndex.text != "") {
                int index = int.Parse(variableArrayIndex.text);
                consoleValue = input;

                if (index > _stringArray.First().Value.stringArr.Length - 1) {
                    consoleValue = "Array index out of bounds";
                    error = true;
                    // if (_stringArray.First().Value.stringArr.Length > 0)  {
                    //     _stringArray.First().Value.stringArr[index] = "Array index out of bounds";
                    //     if (originalObj != null) originalObj.GetComponent<BlockVariable>()._stringArray.First().Value.stringArr[index] = "Array index out of bounds";
                    // }
                }
                else {
                    error = false;
                    _stringArray.First().Value.stringArr[index] = input;
                    if (originalObj != null) originalObj.GetComponent<BlockVariable>()._stringArray.First().Value.stringArr[index] = input;
                }
            }
        }
        else if (_stringVar.Count() > 0) {
            // Get the first key in the dictionary
            string firstKey = _stringVar.Keys.First();
            consoleValue = input;

            _stringVar.Remove(firstKey);
            _stringVar.Add(firstKey, input);

            if (originalObj != null) {
                originalObj.GetComponent<BlockVariable>()._stringVar.Remove(firstKey);
                originalObj.GetComponent<BlockVariable>()._stringVar.Add(firstKey, input);
                originalObj.GetComponent<BlockDrag>().consoleValue = input;
            }
        }
        else if (_intVar.Count() > 0) {
            if (int.TryParse(input, out int value)) {
                error = false;
                // Get the first key in the dictionary
                string firstKey = _intVar.Keys.First();
                consoleValue = input;
                _intVar.Remove(firstKey);
                _intVar.Add(firstKey, int.Parse(input));

                if (originalObj != null) {
                    originalObj.GetComponent<BlockVariable>()._intVar.Remove(firstKey);
                    originalObj.GetComponent<BlockVariable>()._intVar.Add(firstKey, int.Parse(input));
                    originalObj.GetComponent<BlockDrag>().consoleValue = input;
                }
            }
            else {
                error = true;
            }
        }

        if (originalObj != null) originalObj.GetComponent<BlockDrag>().inputChanged = true;
        inputChanged = true;
    }

    public override void BlockValidation()
    {
        if (_dropZone == null || !inputChanged) return; // Don't check the validation when not on the drop block

        foreach (var dropID in _dropZone.GetComponent<BlockDrop>().ids)
        {
            if (dropID == id)
            {
                if (_intArray.Count() > 0) {
                    if (variableArrayIndex.text != "") {
                        int index = int.Parse(variableArrayIndex.text);

                        if (index > _intArray.First().Value.intArr.Length - 1) {
                            consoleValue = "Array index out of bounds";
                        }
                        else {
                            consoleValue = _intArray.First().Value.intArr[index].ToString();
                        }
                    }
                }
                else if (_stringArray.Count() > 0) {
                    if (variableArrayIndex.text != "") {
                        int index = int.Parse(variableArrayIndex.text);

                        if (index > _stringArray.First().Value.stringArr.Length - 1) {
                            consoleValue = "Array index out of bounds";
                        }
                        else {
                            consoleValue = _stringArray.First().Value.stringArr[index];
                        }
                    }
                }
                else if (_stringVar.Count() > 0) {
                    consoleValue = _stringVar.Values.First().ToString();
                }
                else if (_intVar.Count() > 0) {
                    consoleValue = _intVar.Values.First().ToString();
                }

                if (ValidateInput())
                {
                    if (!addedPoints && addPoints)
                    {
                        validationManager.AddPoints(1);
                        addedPoints = true;
                    }
                }
                inputChanged = false;
                return;
            }
            else {
                if (addedPoints && addPoints)
                {
                    validationManager.ReducePoints(1);
                    addedPoints = false;
                }
            }
        }

        inputChanged = false;
    }

    private void ResetVariables() {
        if (_intArray.Count() > 0) {
            _intArray[_intArray.First().Key].intArr = new int[0];
        }
        if (_stringArray.Count() > 0) {
            _stringArray[_stringArray.First().Key].stringArr = new string[0];
        }
        if (_stringVar.Count() > 0) {
            _stringVar.Clear();
            foreach (var val in _defStringVar) {
                _stringVar.Add(val.Key, val.Value);
            }
        }
        if (_intVar.Count() > 0) {
            _intVar.Clear();
            foreach (var val in _defIntVar) {
                _intVar.Add(val.Key, val.Value);
            }
        }

        validationManager.resetBlocks = false;  
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
