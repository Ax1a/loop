using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetupFunction : BlockSetup
{
    [SerializeField] private TMP_Dropdown returnType;
    [SerializeField] private TMP_InputField functionName;
    [SerializeField] private GameObject parameterPrefab;
    [SerializeField] private Transform parameterParent;

    public override bool Validate() {

        return false;
    }

    public void AddParameter() {

    }

    public void RemoveParameter() {

    }
}