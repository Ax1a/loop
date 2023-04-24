using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LessonDropBlock : MonoBehaviour, IDropHandler
{
    public int id;
    public bool _pointsAdded;
    public LessonDragBlock.BlockType blockType;
    [HideInInspector] public LessonDragBlock drag;
    public static LessonDropBlock Instance;
    bool isCorrect = false;
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        _pointsAdded = false;
    }
    void Update()
    {
        if (slot)
        {
            GetComponent<Image>().enabled = false;
        }
        else
        {
            GetComponent<Image>().enabled = true;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        LessonDragBlock dragObject = eventData.pointerDrag.GetComponent<LessonDragBlock>();
        if (dragObject.tag == "Block")
        {
            /*
                Check the block type of the blocks
                if block type is same, it will snap to position of blank spaces.
                if not, it will reset the position. 
            */

            //This condition checks if there is a child inside the droppable object
            if (!slot)
            {
                if (dragObject.blockType == blockType)
                {
                    /* dragObject.GetComponent<RectTransform>().transform.position =
                       GetComponent<RectTransform>().transform.position; */
                    CheckAnswer(eventData);
                    dragObject.parentToReturn = transform;
                    AudioManager.Instance.PlaySfx("Pop");
                    Debug.Log("Dropped");


                }
                else
                {
                    dragObject.GetComponent<LessonDragBlock>().ResetPos();
                    Debug.Log("Block type do not match");
                    AudioManager.Instance.PlaySfx("Pop");
                }
            }
        }
        LayoutRefresher.Instance.RefreshContentFitter(transform as RectTransform);
    }
    public void CheckAnswer(PointerEventData eventData)
    {
        Debug.Log("CheckAnswer Function is Called");
        //Check the correct answer through ID of the blocks
        if (eventData.pointerDrag.GetComponent<LessonDragBlock>().id == id)
        {

            if (!isCorrect) // if the block has not been correctly placed before
            {
                LessonDragDropValidation.Instance.AddPoints(); // add points
                isCorrect = true; // set isCorrect to true
                Debug.Log("correct: " + isCorrect);
            }
        }
        else
        {
            Debug.Log("Wrong");
            if (isCorrect) // if the block was previously correctly placed
            {
                LessonDragDropValidation.Instance.MinusPoints(); // deduct points
                isCorrect = false; // set isCorrect back to false
                Debug.Log("correct: " + isCorrect);
            }
        }
    }

    public void Reset()
    {
        isCorrect = false;
    }
}
