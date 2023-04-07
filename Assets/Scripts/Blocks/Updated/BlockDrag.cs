using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header ("FOR SIDE PANEL ONLY")]
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private bool instantiate = false;
    private GameObject currentDrag;
    private Transform _tempParent;
    private Transform _environmentParent;
    private GameObject _environmentContainer;

    private void Awake() {
        _tempParent = GameObject.FindGameObjectWithTag("BlockTempParent").transform;
        _environmentParent =  GameObject.FindGameObjectWithTag("BlockEnvironmentParent").transform;
        _environmentContainer = GameObject.FindGameObjectWithTag("BlockEnvironment");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Create a new instance of the block prefab and set it as the current drag object
        if (instantiate) {
            currentDrag = Instantiate(blockPrefab, transform.position, Quaternion.identity, _tempParent);
            currentDrag.GetComponent<BlockDrag>()._tempParent = _tempParent;
            currentDrag.GetComponent<BlockDrag>()._environmentParent = _environmentParent;
        }
        else {
            currentDrag = gameObject;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the current drag object to match the mouse position
        currentDrag.transform.position = Input.mousePosition;
        
        if (!instantiate) {
            transform.SetParent(_tempParent.transform);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if the current drag object is within the environment panel's bounds
        if (RectTransformUtility.RectangleContainsScreenPoint(_environmentContainer.GetComponent<RectTransform>(), Input.mousePosition))
        {
            // Set the parent of the current drag object to the environmentParent
            // _environmentParent is the parent of environmentParent.
            currentDrag.transform.SetParent(_environmentParent.transform);
        }
        else
        {
            // Destroy the gameobject when outside of the environment scope
            if (currentDrag != null) {
                Destroy(currentDrag);
            }
            else {
                Destroy(gameObject);
            }
        }
        currentDrag = null;
    }
}
