using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    //private Drop drop; 
  
    //[SerializeField] private Canvas canvas;  

    private void Awake(){
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rect.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
       // Debug.Log(drop.isDrop);
        // Go back to original position if not drop in the right component.. 
        
        // if (dropScript.isDrop == false)
        // {
        //     Debug.Log("false");
        // }else{
        //     Debug.Log("True");
        // }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

   

}
 