using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private Vector3 tooltipOffset;

    private GameObject tooltip;

    private void Update() {
        if (tooltip != null) tooltip.transform.position = Input.mousePosition + tooltipOffset;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip = Instantiate(tooltipPrefab);
        tooltip.transform.SetParent(transform.parent.gameObject.transform.parent, false);
        tooltip.transform.position = Input.mousePosition + tooltipOffset;

        tooltip.GetComponent<BlockTooltip>().SetTooltipText(text);
        tooltip.GetComponent<BlockTooltip>().FadeIn();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.GetComponent<BlockTooltip>().FadeOut();;
    }
}

