using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlockDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int id;
    [HideInInspector] public Transform _environmentParent = null;
    [HideInInspector] public Transform _tempParent = null;
    [HideInInspector] public GameObject _dropZone;
    [HideInInspector] public bool isOverDropZone = false;
    [HideInInspector] public bool addedPoints = false;
    [HideInInspector] public ValidateController validationManager;
    [HideInInspector] public bool inputChanged = true;
    [SerializeField] private bool instantiate = true;
    private Transform refreshParent = null;
    private GameObject _currentDrag;
    private bool deleteObject = false;

    public virtual void Start()
    {
        _tempParent = GameObject.FindGameObjectWithTag("BlockTempParent").transform;
        _environmentParent =  GameObject.FindGameObjectWithTag("BlockEnvironmentParent").transform;
        validationManager = GameObject.FindGameObjectWithTag("ValidateController").GetComponent<ValidateController>();
    }

    public void Update() {
        BlockValidation();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        inputChanged = false;

        // Instantiate a copy of the object and set it as the current drag
        if (instantiate) {
            _currentDrag = Instantiate(gameObject, transform.parent);
        }
        else {
            _currentDrag = gameObject;
        
        }
        _currentDrag.transform.position = transform.position;
        _currentDrag.transform.localScale = transform.localScale;
        _currentDrag.transform.SetParent(_tempParent);
        _currentDrag.GetComponent<CanvasGroup>().blocksRaycasts = false;
        _currentDrag.GetComponent<CanvasGroup>().interactable = false;
        _currentDrag.GetComponent<CanvasGroup>().alpha = 0.6f;
        _currentDrag.GetComponent<BlockDrag>().instantiate = false;

        if (_currentDrag.GetComponent<BlockDrag>()._dropZone != null) {
            Image dropZoneImage = _currentDrag.GetComponent<BlockDrag>()._dropZone.GetComponent<Image>();
            Color color = dropZoneImage.color;
            color.a = 0.5f;
            dropZoneImage.color = color; // Set the new color with alpha 0 to the drop zone image
        }

        if (addedPoints) {
            addedPoints = false;
            validationManager.ReducePoints(1);
        }
        RefreshContentFitter((RectTransform)_environmentParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_currentDrag != null) {
            _currentDrag.transform.position = Input.mousePosition;
            CheckForDropZone();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _currentDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
        _currentDrag.GetComponent<CanvasGroup>().alpha = 1;
        _currentDrag.GetComponent<CanvasGroup>().interactable = true;
        
        if (isOverDropZone)
        {
            if (deleteObject) {
                Destroy(_currentDrag);
                return;
            }

            BlockDrag blockDrag = _currentDrag.GetComponent<BlockDrag>();
            RefreshContentFitter((RectTransform)blockDrag.refreshParent);
            
            if (blockDrag._dropZone != null) {
                _currentDrag.transform.SetParent(blockDrag._dropZone.transform); // Snap the block to the drop zone
                Image dropZoneImage = blockDrag._dropZone.GetComponent<Image>();
                Color color = dropZoneImage.color;
                color.a = 0;
                dropZoneImage.color = color; // Set the new color with alpha 0 to the drop zone image
                
                foreach (var dropID in blockDrag._dropZone.GetComponent<BlockDrop>().ids)
                {
                    if (dropID == id)
                    {
                        inputChanged = true;
                        BlockValidation();
                    }
                }
            }
        }
        else
        {
            _currentDrag.transform.SetParent(_tempParent);
        }
    }

    public virtual void BlockValidation() {}

    public void CheckForDropZone()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult raycastResult in raycastResults)
            {
                if (raycastResult.gameObject.CompareTag("BlockDrop"))
                {
                    BlockDrop dropZone = raycastResult.gameObject.GetComponent<BlockDrop>();
                    if (dropZone.transform.childCount == 0 && dropZone.name != "ChildContainer") {
                        CanvasGroup canvasGroup = dropZone.transform.GetComponent<CanvasGroup>();
                        if (canvasGroup == null || canvasGroup.interactable) {
                            _currentDrag.GetComponent<BlockDrag>()._dropZone = dropZone.gameObject;
                            refreshParent = _currentDrag.transform.parent.transform.parent.transform;
                            isOverDropZone = true;
                        }
                    }
                    else if (dropZone.name == "ChildContainer") {
                        CanvasGroup canvasGroup = dropZone.transform.parent.GetComponentInParent<CanvasGroup>();
                        if (canvasGroup.interactable) {
                            _currentDrag.GetComponent<BlockDrag>()._dropZone = dropZone.gameObject;
                            refreshParent = _currentDrag.transform.parent.transform.parent.transform;
                            isOverDropZone = true;
                        }
                    }
                    deleteObject = true;

                    return;
                }
                else if (raycastResult.gameObject.CompareTag("BlockDelete")) {
                    validationManager.SetTrashIcon(true);
                    Debug.Log("test");
                    // Destroy(_currentDrag);
                    deleteObject = true;
                }
                else {
                    validationManager.SetTrashIcon(false);
                    deleteObject = false;
                }
            }
        }
        isOverDropZone = false;
    }

    public void RefreshContentFitter(RectTransform transform)
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