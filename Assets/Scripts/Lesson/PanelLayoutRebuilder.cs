using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLayoutRebuilder : MonoBehaviour
{
    public RectTransform panelsRectTransforms;
    void Start()
    {

        panelsRectTransforms = GetComponent<RectTransform>();

    }
    public static void RebuildLayout(RectTransform panelsRectTransforms)
    {

        LayoutRebuilder.ForceRebuildLayoutImmediate(panelsRectTransforms);
        Debug.Log("Rebuild");
    }

}
