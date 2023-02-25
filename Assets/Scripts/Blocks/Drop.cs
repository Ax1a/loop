using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Drop : MonoBehaviour, IDropHandler 
{
    public int id;
    [SerializeField] public string blockType;
    [HideInInspector] public Drag drag;
    // public Transform simpleBlock;

    // private TMP_InputField varInput;
    // public TextMeshProUGUI log;
    
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
                Debug.Log("Dropped Item");
                //Check the correct answer through ID of the blocks
                if(eventData.pointerDrag.GetComponent<Drag>().id == id)
                {
                    // varInput = simpleBlock.GetComponentInChildren<TMP_InputField>();
                    // string varName = varInput.text;
                    // Debug.Log(varName);  
                    //to-do: if points was added, it will not generate anymore
                    Debug.Log("Correct");
                    GameObject.Find("Win").GetComponent<Win>().AddPoints();
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
