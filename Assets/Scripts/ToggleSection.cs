using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSection : MonoBehaviour
{
    public Transform newVariablePanel;
    public Canvas mainCanvas;
    public Sprite IconExpandDown;
    public Sprite IconExpandUp;

    public Transform BlocksPanel
    {
        get
        {
            if(mainCanvas == null)
            {
                mainCanvas = transform.GetComponentInChildren<Canvas>();
            }
            Transform panel = null;
            foreach (Transform child in mainCanvas.transform)
            {
                if (child.name == "Blocks Scroll View")
                {
                    panel = child.GetChild(0).GetChild(0);
                    break;
                }
            }
            return panel;
        }
    }

    void Start()
    {
        mainCanvas = transform.GetComponentInChildren<Canvas>();
   
    }

    void Update()
    {
    
    }

    public void toggle(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        Transform sectionTransform = button.transform.parent;

        if (buttonImage.sprite.name == "Icon ExpandDown")
        {
            buttonImage.sprite = ExpandSection(sectionTransform);
        }
        else
        {
            buttonImage.sprite = CollapseSection(sectionTransform);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(BlocksPanel.GetComponent<RectTransform>());
    }

    Sprite ExpandSection(Transform sectionTransform)
    {
        for (int i = 1; i < sectionTransform.childCount; i++)
        {
            if (sectionTransform.GetChild(i) != newVariablePanel)
                sectionTransform.GetChild(i).gameObject.SetActive(true);
        }
        return IconExpandUp;
    }

    Sprite CollapseSection(Transform sectionTransform)
    {
        for (int i = 1; i < sectionTransform.childCount; i++)
        {
            sectionTransform.GetChild(i).gameObject.SetActive(false);
        }
        return IconExpandDown;
    }

    
}
