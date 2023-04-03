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
    public Vector2 initPos;
    public BlockData blockData;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        initPos = transform.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        GameObject parent = GameObject.FindGameObjectWithTag("parent");
        transform.SetParent(parent.transform);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");

        parentAfterDrag = transform.parent;
        canvasGroup.blocksRaycasts = true;

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
