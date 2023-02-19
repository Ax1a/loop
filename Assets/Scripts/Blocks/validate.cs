using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class validate : MonoBehaviour
{
    private TMP_InputField input;
    public TextMeshProUGUI output;
    public Transform simpleBlock;
    public Button btn;

    public void valid()
    {
        input = simpleBlock.GetComponentInChildren<TMP_InputField>();
        output.text = input.text;
        btn.transform.gameObject.SetActive(true);     
    }
}
