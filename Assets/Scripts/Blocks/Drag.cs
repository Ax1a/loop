using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [HideInInspector]
    public Transform parentAfterDrag;
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    Vector2 initPos;
    GameObject onDragCanvas;
    public GameObject parent;
    public Transform parentToReturn = null;
    GameObject ghostBlock = null;



    public BlockData blockData;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        initPos = transform.position;
        onDragCanvas = GameObject.FindGameObjectWithTag("onDragCanvas");
        parent = GameObject.FindGameObjectWithTag("parent");
        parentToReturn = parent.transform;

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Creating a placeholder when dragging 
        ghostBlock = new GameObject();
        ghostBlock.transform.SetParent (transform.parent);
        LayoutElement le = ghostBlock.AddComponent<LayoutElement>();
        le.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        le.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        le.flexibleHeight = 0;
        le.flexibleWidth = 0;

        //Making the new created placeholder to be the index of dragging object
        ghostBlock.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(onDragCanvas.transform);
        canvasGroup.blocksRaycasts = false;

        // GameObject parent = GameObject.FindGameObjectWithTag("parent");
        // transform.SetParent(parent.transform);
        // transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");

        // parentAfterDrag = transform.parent;

        transform.SetParent(parentToReturn);

        canvasGroup.blocksRaycasts = true;

        //Destroy invisible block on drag
        Destroy(ghostBlock);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySfx("Pop");
    }
    public void ResetPos()
    {
        transform.position = initPos;
    }


}
