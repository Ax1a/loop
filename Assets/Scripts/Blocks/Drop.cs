using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Drop : MonoBehaviour, IDropHandler 
{

    public bool isDrop  ; 
    public void OnDrop(PointerEventData eventData)
    {

        Debug.Log("OnDrop");
        
        // Debug.Log(isDrop);
        // if (eventData.pointerDrag != null){
        //   isDrop = true;
        // }
      
        if (eventData.pointerDrag != null){
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
 
        }
    }

 
}
