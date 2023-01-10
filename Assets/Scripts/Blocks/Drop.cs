using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Drop : MonoBehaviour, IDropHandler 
{
    public int id;
    [SerializeField] public string blockType;

    [HideInInspector] public Drag drag;

  
    
    public void OnDrop(PointerEventData eventData)
    {
        
        if (eventData.pointerDrag != null){

            //Check the block type of the blocks
            //if block type is same, it will snap to position of blank spaces.
            //if not, it will reset the position.  
            if(eventData.pointerDrag.GetComponent<Drag>().blockType == blockType)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().transform.position =
                GetComponent<RectTransform>().transform.position;
                // GameObject dropped = eventData.pointerDrag;
                // Drag drag = dropped.GetComponent<Drag>();
                // drag.parentAfterDrag = transform;
                Debug.Log("Dropped Item");


                Debug.Log("OnDrop");

            //Check the correct answer through ID of the blocks
                if(eventData.pointerDrag.GetComponent<Drag>().id == id)
                {
                    //to-do: if points was added, it will not generate anymore
                    Debug.Log("Correct");
                    GameObject.Find("Win").GetComponent<Win>().AddPoints();
                }
            }
            else
            {
                eventData.pointerDrag.GetComponent<Drag>().ResetPos();
                Debug.Log("ResetPosition");
            }
       

            // else{
            //         Debug.Log("Wrong");
            //         GameObject.Find("Win").GetComponent<Win>().MinusPoints();
            // }
        }
    }
}
