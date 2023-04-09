using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;
    [SerializeField] private GameObject tooltipPrefab;
    private GameObject tooltip;
    private BlockTooltip tooltipScript;
    private Coroutine tooltipCoroutine;

    private void Update() {
        if (tooltip != null) tooltip.transform.position = Input.mousePosition + tooltipScript.tooltipOffset;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipCoroutine = StartCoroutine(ShowTooltip());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipCoroutine != null) StopCoroutine(tooltipCoroutine);
        if (tooltipScript != null) tooltipScript.FadeOut();
    }

    private IEnumerator ShowTooltip()
    {
        yield return new WaitForSeconds(.7f);
        tooltip = Instantiate(tooltipPrefab);
        tooltipScript = tooltip.GetComponent<BlockTooltip>();
        tooltip.transform.SetParent(transform.parent.gameObject.transform.parent, false);
        tooltip.transform.position = Input.mousePosition + tooltipScript.tooltipOffset;
        tooltipScript.SetTooltipText(text);
        tooltipScript.FadeIn();
    }
}

