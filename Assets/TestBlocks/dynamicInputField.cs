using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dynamicInputField : MonoBehaviour
{

    public TMP_InputField inputField;
    public RectTransform rectTransform;
    public float padding = 10f;
    public float minHeight = 30f;

    private void Start()
    {
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(string value)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        float preferredHeight = LayoutUtility.GetPreferredHeight(inputField.textComponent.rectTransform) + padding;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Mathf.Max(preferredHeight, minHeight));
    }
}
