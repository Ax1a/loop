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
    [SerializeField] public IntVariableArr _intArray;
    [SerializeField] public StringVariableArr _stringArray;
    [SerializeField] public StringVariable _stringVar;
    [SerializeField] public IntVariable _intVar;

    [Header ("Objects")]
    [SerializeField] private TextMeshProUGUI variableNameTxt;
    [SerializeField] private TMP_InputField variableArrayIndex;
    private BlockVariable origBlockVariable = null;

    public override void Start() {
        base.Start();

        variableArrayIndex.onValueChanged.AddListener(OnInputFieldValueChanged);
        if (_intArray.Count() > 0) {
            variableNameTxt.text = _intArray.Keys.First().ToString();
            variableArrayIndex.transform.parent.gameObject.SetActive(true);
        }
        else if (_stringArray.Count() > 0) {
            variableNameTxt.text = _stringArray.Keys.First().ToString();
            variableArrayIndex.transform.parent.gameObject.SetActive(true);
        }
        else if (_stringVar.Count() > 0) {
            variableArrayIndex.transform.parent.gameObject.SetActive(false);
            variableNameTxt.text = _stringVar.Keys.First().ToString();
        }
        else if (_intVar.Count() > 0) {
            variableArrayIndex.transform.parent.gameObject.SetActive(false);
            variableNameTxt.text = _intVar.Keys.First().ToString();
        }
        inputChanged = true;
    }

    public override void Update() {
        base.Update();

        if (origBlockVariable != null && !instantiate) {
            Debug.Log(originalObj.name);
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
            else if (_stringVar.Count() > 0 && _stringVar.Values.First().ToString() != consoleValue) {
                _stringVar = origBlockVariable._stringVar;
                consoleValue = _stringVar.Values.First().ToString();
            }
            else if (_intVar.Count() > 0 && _intVar.Values.First().ToString() != consoleValue) {
                _intVar = origBlockVariable._intVar;
                consoleValue = origBlockVariable._intVar.Values.First().ToString();
            }

            if (originalObj.GetComponent<BlockDrag>().inputChanged) {
                inputChanged = true;
                originalObj.GetComponent<BlockDrag>().inputChanged = false;

                if (_stringVar.Count() > 0) {
                    _stringVar = origBlockVariable._stringVar;
                    consoleValue = _stringVar.Values.First().ToString();
                }
                 else if (_intVar.Count() > 0) {
                    _intVar = origBlockVariable._intVar;
                    consoleValue = origBlockVariable._intVar.Values.First().ToString();
                }
            }
        }
        else if (origBlockVariable == null && !instantiate) {
            if (originalObj != null) origBlockVariable = originalObj.GetComponent<BlockVariable>();
            inputChanged = true;
        }
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

    public void SetDictionaryValue(string input) {
        if (_intArray.Count() > 0) {
            if (variableArrayIndex.text != "") {
                int index = int.Parse(variableArrayIndex.text);
                consoleValue = input;

                if (index > _intArray.First().Value.intArr.Length - 1) {
                    _intArray.First().Value.intArr[index] = 0;
                    originalObj.GetComponent<BlockVariable>()._intArray.First().Value.intArr[index] = 0;
                    Debug.Log("Array index out of bounds");
                }
                else {
                    _intArray.First().Value.intArr[index] = int.Parse(input);
                    originalObj.GetComponent<BlockVariable>()._intArray.First().Value.intArr[index] = int.Parse(input);
                }
            }
        }
        else if (_stringArray.Count() > 0) {
            if (variableArrayIndex.text != "") {
                int index = int.Parse(variableArrayIndex.text);
                consoleValue = input;

                if (index > _stringArray.First().Value.stringArr.Length - 1) {
                    _stringArray.First().Value.stringArr[index] = "Array index out of bounds";
                    originalObj.GetComponent<BlockVariable>()._stringArray.First().Value.stringArr[index] = "Array index out of bounds";
                }
                else {
                    _stringArray.First().Value.stringArr[index] = input;
                    originalObj.GetComponent<BlockVariable>()._stringArray.First().Value.stringArr[index] = input;
                }
            }
        }
        else if (_stringVar.Count() > 0) {
            // Get the first key in the dictionary
            string firstKey = _stringVar.Keys.First();
            consoleValue = input;

            _stringVar.Remove(firstKey);
            _stringVar.Add(firstKey, input);

            originalObj.GetComponent<BlockVariable>()._stringVar.Remove(firstKey);
            originalObj.GetComponent<BlockVariable>()._stringVar.Add(firstKey, input);
            originalObj.GetComponent<BlockDrag>().consoleValue = input;
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
            
            if (dropID == id)
            {
                if (!addedPoints && addPoints)
                {
                    validationManager.AddPoints(1);
                    addedPoints = true;
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
