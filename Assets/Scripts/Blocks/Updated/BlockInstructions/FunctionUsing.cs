using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FunctionUsing : Block
{
    public TMP_InputField usingInput;

    public override bool Validate() {
        if (usingInput.text == "namespace std") {
            return true;
        }

        return false;
    }
}