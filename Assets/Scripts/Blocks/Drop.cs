using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class Drop : MonoBehaviour, IDropHandler
{
    public bool _pointAdded = false;
    public BlockData blockData;
    [HideInInspector] public Drag drag;
    [SerializeField] private GameObject validationManager;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            /*
                Check the block type of the blocks
                if block type is same, it will snap to position of blank spaces.
                if not, it will reset the position. 
            */
            if (eventData.pointerDrag.GetComponent<Drag>().blockData.blockType == blockData.blockType)
            {
                eventData.pointerDrag.GetComponent<RectTransform>().transform.position = GetComponent<RectTransform>().transform.position;
                AudioManager.Instance.PlaySfx("Pop");
                Validate(eventData);
            }
            else
            {
                eventData.pointerDrag.GetComponent<Drag>().ResetPos();
                Debug.Log("Block type do not match");
                AudioManager.Instance.PlaySfx("Pop");
            }
        }
    }
    public void Validate(PointerEventData eventData)
    {
        //Check the correct answer through ID of the blocks
        if (eventData.pointerDrag.GetComponent<Drag>().blockData.id == blockData.id)
        {
            //to-do: if points was added, it will not generate anymore
            if (!_pointAdded)
            {
                Debug.Log("Correct");
                validationManager.GetComponent<Win>().AddPoints();
                _pointAdded = true;
            }
            else
            {
                Debug.Log("Points Already Added!");
            }
        }
        else
        {
            Debug.Log("Wrong");
        }
    }
}
