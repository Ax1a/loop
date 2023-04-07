using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateVariable : MonoBehaviour
{
    // public Transform varPanel;

    [SerializeField] GameObject newVar;

    [SerializeField] TMP_InputField varInput;

    [SerializeField] Transform newVarParent;

    string varName;

    // public void createVar()
    // {
    //     varPanel.gameObject.SetActive(true);
    // }
    public void CreateNewVariable()
    {
        varName = varInput.text;
        newVar.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = varName;
        Debug.Log("The variable name is: " + varName);
        newVar.name = "variable";
        Instantiate(newVar, newVarParent);
    }
}
