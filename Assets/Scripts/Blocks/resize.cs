using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resize : MonoBehaviour
{
    public InputField inputField;
    private RectTransform rectTransform;
    int offsetInputFieldSize;
    int offsetDropdownSizeBig;
    int minDropdownSize;

    public RectTransform RectTransform
    {
        get => rectTransform;
        set
        {
            if (GetComponent<InputField>())
            {
                inputField = GetComponent<InputField>();
                inputField.onValueChanged.AddListener(delegate { ExpandInputField(); });
            }
            
            rectTransform = value;
        }
    }

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        offsetInputFieldSize = 35;
        offsetDropdownSizeBig = 35;
        minDropdownSize = 100;
    }

    public void ResizeInputField()
    {
        offsetInputFieldSize = 35;
        offsetDropdownSizeBig = 35;
        minDropdownSize = 100;

        if (GetComponent<InputField>())
        {
            ExpandInputField();
        }
    }

    public void ExpandInputField()
    {
        RectTransform.sizeDelta = new Vector2(offsetInputFieldSize + inputField.preferredWidth, RectTransform.sizeDelta.y);
        try
        {
            // BELayoutRebuild.RebuildAll();
        }
        catch { }
    }
}
