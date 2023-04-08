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
    

    private void Awake() {
        _tempParent = GameObject.FindGameObjectWithTag("BlockTempParent").transform;
        _environmentParent =  GameObject.FindGameObjectWithTag("BlockEnvironmentParent").transform;
        _environmentContainer = GameObject.FindGameObjectWithTag("BlockEnvironment");
        _deleteBlockContainer = GameObject.FindGameObjectWithTag("BlockDelete");
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
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the current drag object to match the mouse position
        _currentDrag.transform.position = Input.mousePosition;
        
        if (!instantiate && transform.parent != _tempParent) {
            transform.SetParent(_tempParent.transform);
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
            Destroy(_currentDrag);
            BlockController.Instance.SetTrashIcon(false);
            return;
        }

        // Check if the current drag object is within the environment panel's bounds
        if (RectTransformUtility.RectangleContainsScreenPoint(_environmentContainer.GetComponent<RectTransform>(), Input.mousePosition))
        {
            // Get the mouse position in local coordinates relative to the environment panel
            Vector2 localMousePos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_environmentParent.GetComponent<RectTransform>(), Input.mousePosition, eventData.pressEventCamera, out localMousePos))
            {
                // Find the closest child of the environment parent to the mouse position
                Transform closestChild = null;
                float closestDistance = float.MaxValue;
                foreach (Transform child in _environmentParent.transform)
                {
                    float distance = Vector2.Distance(localMousePos, child.GetComponent<RectTransform>().anchoredPosition);

                    // Check the height difference between the current drag object and the child
                    float heightDifference = Mathf.Abs(child.GetComponent<RectTransform>().sizeDelta.y - _currentDrag.GetComponent<RectTransform>().sizeDelta.y);

                    // Calculate the total distance, taking into account the height difference
                    float totalDistance = Mathf.Sqrt(Mathf.Pow(distance, 2) + Mathf.Pow(heightDifference, 2));

                    if (totalDistance < closestDistance)
                    {
                        closestChild = child;
                        closestDistance = totalDistance;
                    }
                }

                // Set the parent and sibling index of the current drag object
                if (closestChild != null)
                {
                    _currentDrag.transform.SetParent(_environmentParent.transform, false);
                    _currentDrag.transform.SetSiblingIndex(closestChild.GetSiblingIndex() + 1);
                }
                else
                {
                    _currentDrag.transform.SetParent(_environmentParent.transform, false);
                    _currentDrag.transform.SetAsLastSibling();
                }
            }
        }
        else
        {
            // Destroy the gameobject when outside of the environment scope
            if (_currentDrag != null && instantiate)
            {
                Destroy(_currentDrag);
            }
            else {
                _currentDrag.transform.SetParent(_environmentParent.transform);
            }
        }

        _currentDrag = null;
    }
}
