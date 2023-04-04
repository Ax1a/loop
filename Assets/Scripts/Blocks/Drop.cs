using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Drop : MonoBehaviour, IDropHandler
{
    public BlockData blockData;
    [HideInInspector] public Drag drag;
    [SerializeField] private GameObject validationManager;

    public GameObject slot
    {
        get
        {
            if (transform.childCount > 0)
            {
                return this.transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Drag dragObject = eventData.pointerDrag.GetComponent<Drag>();
        Debug.Log(dragObject.name + " was dropped on " + gameObject.name);

        if (dragObject.tag == "Block")
        {
            /*
                Check the block type of the blocks
                if block type is same, it will snap to position of blank spaces.
                if not, it will reset the position. 
            */
            if (!slot)
            {
                if (dragObject.blockData.blockType == blockData.blockType)
                {
                    // eventData.pointerDrag.GetComponent<RectTransform>().transform.position = GetComponent<RectTransform>().transform.position;
                    dragObject.parentToReturn = transform;
                    AudioManager.Instance.PlaySfx("Pop");
                    Validate(eventData);
                }
                else
                {
                    dragObject.GetComponent<Drag>().ResetPos();
                    Debug.Log("Block type do not match");
                    AudioManager.Instance.PlaySfx("Pop");
                }
            }
        }
    }
    public void Validate(PointerEventData eventData)
    {
        //Check the correct answer through ID of the blocks
        if (eventData.pointerDrag.GetComponent<Drag>().blockData.id == blockData.id)
        {
            //to-do: if points was added, it will not generate anymore
            Debug.Log("Correct");
            Win.Instance.AddPoints();
            // validationManager.GetComponent<Win>().AddPoints();
        }
        else
        {
            Win.Instance.MinusPoints();
            // validationManager.GetComponent<Win>().MinusPoints();
            Debug.Log("Wrong");
        }
    }
}
