using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicInputResize : MonoBehaviour
{
    public InputField inputField;
    public Dropdown dropdown;
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
            if (GetComponent<Dropdown>())
            {
                dropdown = GetComponent<Dropdown>();
                dropdown.onValueChanged.AddListener(delegate { ExpandDropdown(); });
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
        else if (GetComponent<Dropdown>())
        {
            ExpandDropdown();
        }
    }

    public void ExpandInputField()
    {
        RectTransform.sizeDelta = new Vector2(offsetInputFieldSize + inputField.preferredWidth, RectTransform.sizeDelta.y);
        try
        {
            BELayoutRebuild.RebuildAll();
        }
        catch { }
    }

    public void ExpandDropdown()
    {
        float newSize = offsetDropdownSizeBig + dropdown.captionText.preferredWidth;
        if (newSize < minDropdownSize)
            newSize = minDropdownSize;
        RectTransform.sizeDelta = new Vector2(newSize, RectTransform.sizeDelta.y);
        try
        {
            BELayoutRebuild.RebuildAll();
        }
        catch { }
    }
}
