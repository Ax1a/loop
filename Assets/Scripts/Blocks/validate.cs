using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class validate : MonoBehaviour
{
    private TMP_InputField input;
    public GameObject errorMessage;
    public TextMeshProUGUI output;
    public Transform simpleBlock;
    public Button btn;

    public void valid()
    {
        input = simpleBlock.GetComponentInChildren<TMP_InputField>();
        if(input.text.Length != 0)
        {
        output.text = input.text;
        btn.transform.gameObject.SetActive(true);     
        }
        else
        {
        errorMessage.SetActive(true);
        Debug.Log("Error Message");
        
        }
    }

    public void removeError()
    {
        if(errorMessage.activeSelf)
        {
        errorMessage.SetActive(false);

        }

    }
}
