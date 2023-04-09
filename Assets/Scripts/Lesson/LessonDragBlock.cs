using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LessonDragBlock : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rect;
    CanvasGroup canvasGroup;
    private Vector2 initPos;
    public int id;
    public GameObject parent;
    GameObject onDragCanvas;
    GameObject ghostBlock = null;
    public Transform parentToReturn = null;
    public enum BlockType
    {
        normalBlock, operation, simple, conditional
    }
    public BlockType blockType;
    public static LessonDragBlock Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        parent = GameObject.FindGameObjectWithTag("parent");
        onDragCanvas = GameObject.FindGameObjectWithTag("onDragCanvas");
        initPos = transform.position;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentToReturn = parent.transform;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Creating a placeholder when dragging 
        ghostBlock = new GameObject();
        ghostBlock.transform.SetParent(transform.parent);
        LayoutElement le = ghostBlock.AddComponent<LayoutElement>();
        le.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        le.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        le.flexibleHeight = 0;
        le.flexibleWidth = 0;

        //Making the new created placeholder to be the index of dragging object
        ghostBlock.transform.SetSiblingIndex(transform.GetSiblingIndex());

        //Make onCanvas to be the parent on drag. 
        transform.SetParent(onDragCanvas.transform);

        //Turning off raycasts to detect object under the dragging object
        canvasGroup.blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        //Set the parent to its original parent if not dropped in right place
        transform.SetParent(parentToReturn);

        //Destroy the ghostblock after dragging
        Destroy(ghostBlock);
        
        RefreshContentFitters();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySfx("Pop");
    }
    public void ResetPos()
    {
        transform.position = initPos;
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
