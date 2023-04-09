using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetupInclude : BlockSetup
{
    public TMP_InputField includeInput;

    public override bool Validate() {
        if (includeInput.text == "<iostream>") {
            return true;
        }

        return false;
    }
}
