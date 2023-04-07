using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FunctionInclude : Block
{
    public TMP_InputField includeInput;

    private void Update() {
        if (includeInput.text == "namespace std") {
            Debug.Log(includeInput.text);
        }
    }
}
