using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragBlock : MonoBehaviour
{
    [SerializeField]private Canvas canvas;

    public void DragHandler(BaseEventData data) {
        PointerEventData pointerData = (PointerEventData)data;
        
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    private void OnTriggerEnter2D (Collider2D other) {
        Debug.Log("works");
        if(other.gameObject.tag == "Block") {
            other.transform.SetParent(this.transform);
            
        }
    }


    private void OnTriggerExit2D (Collider2D other) {
        if(other.gameObject.tag == "Block") {
             other.transform.SetParent(null);
        }
    }
}
