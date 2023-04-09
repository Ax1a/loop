using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetupFunction : BlockSetup
{
    [SerializeField] private TMP_Dropdown returnType;
    [SerializeField] private TMP_InputField functionName;
    [SerializeField] private GameObject parameterPrefab;
    [SerializeField] private Transform parameterParent;
    public List<GameObject> parameters;

    public override bool Validate() {

        return false;
    }

    public void AddParameter() {
        parameters.Add(Instantiate(parameterPrefab, parameterParent));
        RefreshContentFitters();
    }

    public void RemoveParameter() {
        if (parameterParent.childCount != 0) {
            Destroy(parameters[parameterParent.childCount - 1]);
            parameters.RemoveAt(parameterParent.childCount - 1);
            RefreshContentFitters();
        }
    }

    public void RefreshContentFitters()
    {
        var rectTransform = (RectTransform)transform;
        RefreshContentFitter(rectTransform);
    }

    private void RefreshContentFitter(RectTransform transform)
    {
        if (transform == null || !transform.gameObject.activeSelf)
        {
            return;
        }
   
        foreach (RectTransform child in transform)
        {
            RefreshContentFitter(child);
        }
 
        var layoutGroup = transform.GetComponent<LayoutGroup>();
        var contentSizeFitter = transform.GetComponent<ContentSizeFitter>();
        if (layoutGroup != null)
        {
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();
        }
 
        if (contentSizeFitter != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
        }
    }
}