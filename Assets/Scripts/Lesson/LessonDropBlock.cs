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

    public void OnDrop(PointerEventData eventData)
    {
        LessonDragBlock dragObject = eventData.pointerDrag.GetComponent<LessonDragBlock>();
        Debug.Log(dragObject.name + " was dropped on " + gameObject.name);
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
                    dragObject.parentToReturn = transform;
                    AudioManager.Instance.PlaySfx("Pop");
                    Debug.Log("Dropped");
                    CheckAnswer(eventData);
                }
                else
                {
                    eventData.pointerDrag.GetComponent<LessonDragBlock>().ResetPos();
                    // dragObject.transform.SetParent(dragObject.parent.transform);
                    LessonDragDropValidation.Instance.MinusPoints();
                    Debug.Log("Block type do not match");
                    AudioManager.Instance.PlaySfx("Pop");
                    Debug.Log("LessonDropBlockScript: Reset Position");
                }
            }
        }
        RefreshContentFitter((RectTransform) gameObject.transform);

    }
    public void CheckAnswer(PointerEventData eventData)
    {
        //Check the correct answer through ID of the blocks
        if (eventData.pointerDrag.GetComponent<LessonDragBlock>().id == id)
        {
            //to-do: if points was added, it will not generate anymore
            Debug.Log("Correct");
            LessonDragDropValidation.Instance.AddPoints();
            _pointsAdded = true;
            Debug.Log(_pointsAdded);
        }
        else
        {
            // GameObject.Find("Win").GetComponent<Win>().MinusPoints();
            Debug.Log("Wrong");
            LessonDragDropValidation.Instance.MinusPoints();
            Debug.Log(_pointsAdded);
        }
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
