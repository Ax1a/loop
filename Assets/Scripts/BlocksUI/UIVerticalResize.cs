using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIVerticalResize : MonoBehaviour, IDragHandler
{
    BEUIController beUIController;
    RectTransform resizablePanelRectT;

    // x-min size, y-max size
    Vector2 panelLimits;

    void Start()
    {
        beUIController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BEUIController>();
        resizablePanelRectT = GetResizablePanel(transform);
    }

    RectTransform GetResizablePanel(Transform child)
    {
        RectTransform parentRectT;
        if (child.parent.GetComponent<ScrollRect>())
        {
            parentRectT = child.parent.GetComponent<RectTransform>();
        }
        else
        {
            parentRectT = GetResizablePanel(child.parent);
        }
        return parentRectT;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // seting limits related to panel position and screen size
        panelLimits = new Vector2(resizablePanelRectT.offsetMin.x + 180, Screen.width/beUIController.uiScale - 120);

        // resize panel
        Vector2 panelSize = resizablePanelRectT.offsetMax;
        panelSize.x = Input.mousePosition.x / beUIController.uiScale;
        if (panelSize.x < panelLimits.x)
        {
            resizablePanelRectT.offsetMax = new Vector2(panelLimits.x, panelSize.y);
        }
        else if(panelSize.x > panelLimits.y)
        {
            resizablePanelRectT.offsetMax = new Vector2(panelLimits.y, panelSize.y);
        }
        else
        {
            resizablePanelRectT.offsetMax = panelSize;
        }
    }
}
