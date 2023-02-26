using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class DropBlock : MonoBehaviour, IDropHandler 
{
    public int id;
    [SerializeField] public string blockType;
    [HideInInspector] public Drag drag;
    public void OnDrop(PointerEventData eventData)
    { 
        if (eventData.pointerDrag != null){
            if(eventData.pointerDrag.GetComponent<Drag>().blockType == blockType)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().transform.position =
                GetComponent<RectTransform>().transform.position;
                Debug.Log("Dropped Item");
                //Check the correct answer through ID of the blocks
                if(eventData.pointerDrag.GetComponent<Drag>().id == id)
                { 
                    Debug.Log("Correct");
                }
                else
                {
                    Debug.Log("Wrong");
                }
            }
            else
            {
                eventData.pointerDrag.GetComponent<Drag>().ResetPos();
                Debug.Log("ResetPosition");
            }
        }  
    }
}
