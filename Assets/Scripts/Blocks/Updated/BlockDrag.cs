using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlockDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header ("FOR SIDE PANEL ONLY")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private bool instantiate = false;
    private GameObject _currentDrag;
    private Transform _tempParent;
    private Transform _environmentParent;
    private GameObject _deleteBlockContainer;
    private GameObject _environmentContainer;
    private GameObject _shadow;

    private void Awake() {
        _tempParent = GameObject.FindGameObjectWithTag("BlockTempParent").transform;
        _environmentParent =  GameObject.FindGameObjectWithTag("BlockEnvironmentParent").transform;
        _environmentContainer = GameObject.FindGameObjectWithTag("BlockEnvironment");
        _deleteBlockContainer = GameObject.FindGameObjectWithTag("BlockDelete");
    }

    private void Start() {
        if (!BlockController.Instance.childContainers.Contains(_environmentParent)) BlockController.Instance.childContainers.Add(_environmentParent);    
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Create a new instance of the block prefab and set it as the current drag object
        if (instantiate) {
            _currentDrag = Instantiate(blockPrefab, transform.position, Quaternion.identity, _tempParent);
            _currentDrag.GetComponent<BlockDrag>()._tempParent = _tempParent;
            _currentDrag.GetComponent<BlockDrag>()._environmentParent = _environmentParent;
        }
        else {
            _currentDrag = gameObject;
        }

        CreateBlockShadow();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the current drag object to match the mouse position
        _currentDrag.transform.position = Input.mousePosition;
        
        if (!instantiate && transform.parent != _tempParent) {
            transform.SetParent(_tempParent.transform);
        }

        // Check if the current drag object is within the environment panel's bounds
        if (RectTransformUtility.RectangleContainsScreenPoint(_environmentParent.GetComponent<RectTransform>(), Input.mousePosition))
        {
            SnapDraggedBlock(eventData, _shadow);
            _shadow.GetComponent<RectTransform>().sizeDelta = _currentDrag.GetComponent<RectTransform>().sizeDelta;
        }
        else {
            _shadow.SetActive(false);
        }
        
        /* 
            IGNORE
            For changing the icon of the trash only
            This will indicate if the user is near the trash container and can delete the gameobject
        */
        if (_deleteBlockContainer == null) return;

        bool isOpen = false;
        isOpen = RectTransformUtility.RectangleContainsScreenPoint(_deleteBlockContainer.GetComponent<RectTransform>(), Input.mousePosition) ? true : false;
        BlockController.Instance.SetTrashIcon(isOpen);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // If the mouse of the user is near the trash container, then delete the block and reset the icon to closed
        if (RectTransformUtility.RectangleContainsScreenPoint(_deleteBlockContainer.GetComponent<RectTransform>(), Input.mousePosition))
        {
            Block block = _currentDrag.GetComponent<Block>();

            foreach (Transform item in block.childBlockParent)
            {
                if (BlockController.Instance.childContainers.Contains(item)) {
                    BlockController.Instance.childContainers.Remove(item);
                }
            }
            Destroy(_currentDrag);
            BlockController.Instance.SetTrashIcon(false);
            if (_shadow != null) Destroy(_shadow);
            return;
        }

        // Check if the current drag object is within the environment panel's bounds
        if (RectTransformUtility.RectangleContainsScreenPoint(_environmentParent.GetComponent<RectTransform>(), Input.mousePosition))
        {
            SnapDraggedBlock(eventData, _currentDrag);
            Block block = _currentDrag.GetComponent<Block>();
            
            foreach (Transform child in block.childBlockParent)
            {
                if (!BlockController.Instance.childContainers.Contains(child)){ 
                    BlockController.Instance.childContainers.Add(child);
                }
            }
        }
        else
        {
            _currentDrag.transform.SetParent(_tempParent.transform);
        }

        _currentDrag = null;
        if (_shadow != null) Destroy(_shadow);
    }

    // Get the closest child of the environment parent to the mouse position
    private void SnapDraggedBlock(PointerEventData eventData, GameObject setGameObject)
    {
        // Get the mouse position in local coordinates relative to the environment panel
        Vector2 localMousePos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_environmentParent.GetComponent<RectTransform>(), Input.mousePosition, eventData.pressEventCamera, out localMousePos))
        {
            Transform closestChild = null;
            float closestDistance = float.MaxValue;

            // _shadow.SetActive(true);

            // Check snap function for each block type
            if (_currentDrag.GetComponent<Block>().blockType == Block.BlockType.Setup) {
                closestChild = GetClosestChild(closestDistance, localMousePos);
                BlockSetup blockSetup = _currentDrag.GetComponent<BlockSetup>();

                if (closestChild != null) {
                    setGameObject.transform.SetParent(_environmentParent.transform, false);
                    setGameObject.transform.SetSiblingIndex(closestChild.GetSiblingIndex());
                    _shadow.SetActive(true);
                }
            }
            else if (_currentDrag.GetComponent<Block>().blockType == Block.BlockType.NormalBlock || _currentDrag.GetComponent<Block>().blockType == Block.BlockType.Conditional) {
                // BlockNormal blockNormal = _currentDrag.GetComponent<BlockNormal>();
                closestChild = GetClosestChild(closestDistance, localMousePos);

                if (closestChild != null) {
                    Block childBlock = closestChild.GetComponent<Block>();
                    if (childBlock == null) return;
                    Transform parent = null;

                    foreach (Transform item in childBlock.childBlockParent)
                    {
                        if (BlockController.Instance.childContainers.Contains(item)) {
                            parent = item;   
                        }
                    }
                    _shadow.SetActive(true);
                    setGameObject.transform.SetParent(parent, false);
                    setGameObject.transform.SetSiblingIndex(closestChild.GetSiblingIndex());
                }
            }
            else {
                _shadow.SetActive(false);
            }
        }
    }

    private bool IsChildOfContainer(Transform child)
    {
        foreach (Transform container in BlockController.Instance.childContainers)
        {
            if (child.IsChildOf(container))
            {
                return true;
            }
        }

        return false;
    }

    public Transform GetClosestChild(float closestDistance, Vector2 localMousePos)
    {
        Transform closestChild = null;

        foreach (Transform child in _environmentParent)
        {
            if (!IsChildOfContainer(child))
            {
                continue;
            }

            float distance = Vector2.Distance(localMousePos, child.GetComponent<RectTransform>().anchoredPosition);

            // Check the position difference between the current drag object and the child
            float positionDifference = Mathf.Abs(child.GetSiblingIndex() - _currentDrag.transform.GetSiblingIndex());

            // Calculate the total distance, taking into account the position difference
            float totalDistance = Mathf.Sqrt(Mathf.Pow(distance, 2) + Mathf.Pow(positionDifference, 2));

            if (totalDistance < closestDistance)
            {
                closestChild = child;
                closestDistance = totalDistance;
            }
        }

        return closestChild;
    }


    // Shadow to indicate where the block will snap
    public void CreateBlockShadow() {
        _shadow = new GameObject("Shadow", typeof(RectTransform), typeof(Image));
        _shadow.transform.SetParent(_environmentParent.transform);
        _shadow.GetComponent<Image>().sprite = _currentDrag.GetComponent<Image>().sprite;
        _shadow.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        _shadow.GetComponent<Image>().type = Image.Type.Sliced;

        // Set the size of the shadow to match the size of the block being dragged
        _shadow.GetComponent<RectTransform>().localScale = Vector3.one;
        Rect rect = _currentDrag.GetComponent<RectTransform>().rect;
        _shadow.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, rect.height);
        _shadow.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        
        _shadow.transform.position = Input.mousePosition;
        _shadow.SetActive(false);
    }

    // Fix update layout bug
    // public void RefreshContentFitters()
    // {
    //     var rectTransform = (RectTransform)transform;
    //     RefreshContentFitter(rectTransform);
    // }

    // private void RefreshContentFitter(RectTransform transform)
    // {
    //     if (transform == null || !transform.gameObject.activeSelf)
    //     {
    //         return;
    //     }
   
    //     foreach (RectTransform child in transform)
    //     {
    //         RefreshContentFitter(child);
    //     }
 
    //     var layoutGroup = transform.GetComponent<LayoutGroup>();
    //     var contentSizeFitter = transform.GetComponent<ContentSizeFitter>();
    //     if (layoutGroup != null)
    //     {
    //         layoutGroup.SetLayoutHorizontal();
    //         layoutGroup.SetLayoutVertical();
    //     }
 
    //     if (contentSizeFitter != null)
    //     {
    //         LayoutRebuilder.ForceRebuildLayoutImmediate(transform);
    //     }
    // }
}