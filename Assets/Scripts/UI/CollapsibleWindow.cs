using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CollapsibleWindow : MonoBehaviour
{
    [SerializeField] private Image expandIcon;
    [SerializeField] private Image collapseIcon;
    [SerializeField] private GameObject body;
    [SerializeField] private float animationDuration = 0.2f;
    [SerializeField] private ContentSizeFitter parentContentSizeFitter;
    [SerializeField] private LayoutGroup parentLayout;
    
    public void ToggleCollapsibleWindow() {
        bool isActive = body.activeSelf;
        Debug.Log(body.name);

        if (!isActive) {
            body.SetActive(!isActive);
            body.transform.localScale = new Vector3(1, 0, 1);
            body.transform.DOScaleY(1, animationDuration).SetEase(Ease.OutSine);
            RefreshContentFitters();
        }
        else {
            body.transform.DOScaleY(0, animationDuration).SetEase(Ease.InSine).OnComplete(() => {
                body.SetActive(!isActive);
                RefreshContentFitters();
            });
        }
        collapseIcon.gameObject.SetActive(!isActive);
        expandIcon.gameObject.SetActive(isActive);
    }

    // Fix update layout bug
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
