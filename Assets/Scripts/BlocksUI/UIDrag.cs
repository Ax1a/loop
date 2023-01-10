using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IPointerUpHandler
{
    public Vector3 startPosition;
    public int startIndex;
    public UIDrop startParent;
    public Vector3 diffPosition;
    Transform onDragCanvas;
    public BEBlock beBlock;

    RectTransform rectTransform;

    RightClickMenu rightClickMenu;

    GameObject mainCanvas;
    public bool holding;
    public float holdCounter;
    public bool isDragging;
    public bool Draggable;

    BETargetObject TargetObject
    {
        get
        {
            return transform.root.GetComponent<BEProgrammingEnv>().TargetObject;
        }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        //data.pointerDrag = null;
        if (transform.parent.tag != "mainCanvas")
        {
            isDragging = true;
            DragBlock();
        }
    }

    public void RemoveBlockFromParentChildList(BEBlock block)
    {
        if (block.BeBlockGroup != null)
        {
            block.BeBlockGroup.isActive = false;
            block.BeBlockGroup.beActiveBlock = block.BeBlockGroup.GetComponent<BEBlock>();
            block.BeBlockGroup.GetComponent<Outline>().enabled = false;
            block.BeBlockGroup = null;
        }
        block.transform.parent.GetComponent<BEBlock>().beChildBlocksList.Remove(block);
    }

    public void DragBlock()
    {
        BELayoutRebuild.RebuildAll();
        
        try
        {
            startParent = transform.parent.GetComponent<UIDrop>();
        }
        catch
        {
            startParent = null;
        }
        startIndex = transform.GetSiblingIndex();

        startPosition = transform.position;
        diffPosition = Input.mousePosition - startPosition;

        BEBlock block = GetComponent<BEBlock>();

        BEEventSystem.SetSelectedBlock(beBlock);

        if (transform.parent.GetComponent<BEBlock>())
        {
            RemoveBlockFromParentChildList(block);
        }
        else
        {
            if (transform.parent.name == "ProgrammingEnv")
            {
                TargetObject.beBlockGroupsList.Remove(GetComponent<BEBlock>());
            }
        }

        var tempIndex = transform.GetSiblingIndex();

        if (inactiveBlockInputs.Count > 0)
        {
            InactiveBlockInput inactiveChild = inactiveBlockInputs.Find(a => a.childIndex == tempIndex);
            inactiveChild.blockInput.SetParent(transform.parent);
            inactiveChild.blockInput.SetSiblingIndex(tempIndex);
            inactiveChild.blockInput.gameObject.SetActive(true);
            inactiveBlockInputs.Remove(inactiveChild);
        }

        transform.SetParent(onDragCanvas);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (transform.parent.tag == "mainCanvas")
        {
            if (newBlock != null)
            {
                newBlock.transform.position = Input.mousePosition - newBlock.diffPosition;
                newBlock.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                newBlock.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            }
        }
        else
        {
            transform.position = Input.mousePosition - diffPosition;
        }
    }

    UIDrag newBlock;

    IEnumerator InstantiateNewBlock(PointerEventData eventData)
    {
        newBlock = Instantiate(transform).GetComponent<UIDrag>();

        // wait the update of the block size based on the children
        yield return new WaitForEndOfFrame();

        if (newBlock != null)
        {
            Canvas mainCanvas = GameObject.FindGameObjectWithTag("GameController").transform.GetChild(1).GetComponent<Canvas>();
            newBlock.transform.localScale = Vector3.one * mainCanvas.scaleFactor;

            newBlock.transform.position = transform.position;
            newBlock.name = transform.name;
            startPosition = newBlock.transform.position;
            BEEventSystem.SetSelectedBlock(newBlock.beBlock);
            newBlock.transform.SetParent(onDragCanvas);
            newBlock.diffPosition = Input.mousePosition - startPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        beBlock.BeController.ghostBlock.transform.SetParent(null);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OpenRightClickMenu(eventData.position);
        }
    }

    public void OpenRightClickMenu(Vector3 position)
    {
        rightClickMenu.transform.position = position;
        rightClickMenu.target = this;
        rightClickMenu.gameObject.SetActive(true);
    }

    public UIDrop GetParentProgrammingEnv(Transform target)
    {
        if (target.parent != null)
        {
            if (target.parent.name == "ProgrammingEnv")
            {
                return target.parent.GetComponent<UIDrop>();
            }
            else
            {
                return GetParentProgrammingEnv(target.parent);
            }
        }
        else
        {
            return null;
        }
    }

    //stores the children that need to be returned to the last position
    public List<InactiveBlockInput> inactiveBlockInputs = new List<InactiveBlockInput>();

    public void OnPointerDown(PointerEventData eventData)
    {
        if (transform.parent.tag == "mainCanvas")
        {
            StartCoroutine(InstantiateNewBlock(eventData));
        }
        else
        {
            holding = true;
        }
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        beBlock = GetComponent<BEBlock>();
        holding = false;
        isDragging = false;
        Draggable = true;
        holdCounter = 0;
        onDragCanvas = GameObject.FindGameObjectWithTag("onDragCanvas").transform;
        mainCanvas = GameObject.FindGameObjectWithTag("GameController").transform.GetChild(1).gameObject;

        foreach (Transform child in mainCanvas.transform)
        {
            if (child.GetComponent<RightClickMenu>())
            {
                rightClickMenu = child.GetComponent<RightClickMenu>();
            }
        }

        StartCoroutine(BELayoutRebuild.DelayedLayoutRebuild(beBlock));
    }

    void Update()
    {
        if (isDragging == false && holding == true)
        {
            holdCounter += Time.deltaTime;
            if (holdCounter > 0.6f)
            {
                OpenRightClickMenu(Input.mousePosition);
                holdCounter = 0;
            }
        }

        if (Input.GetMouseButton(0) == false)
        {
            if (transform.parent == onDragCanvas)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (transform.parent.tag == "mainCanvas")
        {
            newBlock.holding = false;
            newBlock.holdCounter = 0;
        }
        else
        {
            holding = false;
            holdCounter = 0;
        }
    }
}

public class InactiveBlockInput
{
    public int childIndex;
    public Transform blockInput;

    public InactiveBlockInput(int childIndex, Transform blockInput)
    {
        this.childIndex = childIndex;
        this.blockInput = blockInput;
    }
}
