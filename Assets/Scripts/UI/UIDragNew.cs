using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDragNew : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    Vector3 startPosition;
    Vector3 diffPosition;
    Transform onDragCanvas;

    public void OnDrag(PointerEventData eventData)
    {
        try
        {
            newBlock.GetComponent<UIDrag>().OnDrag(eventData);
        }
        catch { }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    Transform newBlock;

    IEnumerator InstantiateNewBlock()
    {
        newBlock = Instantiate(transform);
        
        // wait the update of the block size based on the children
        yield return new WaitForEndOfFrame();

        if (newBlock != null)
        {
            Canvas mainCanvas = GameObject.FindGameObjectWithTag("GameController").transform.GetChild(1).GetComponent<Canvas>();
            newBlock.localScale = Vector3.one * mainCanvas.scaleFactor;

            newBlock.position = transform.position;
            newBlock.name = transform.name;
            startPosition = newBlock.position;
            UIDrag uIDrag = newBlock.gameObject.AddComponent<UIDrag>();
            BEEventSystem.SetSelectedBlock(uIDrag.beBlock);
            newBlock.transform.SetParent(onDragCanvas);
            newBlock.GetComponent<UIDrag>().diffPosition = Input.mousePosition - startPosition;
            Destroy(newBlock.gameObject.GetComponent<UIDragNew>());
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(InstantiateNewBlock());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        try
        {
            newBlock.GetComponent<UIDrag>().OnEndDrag(eventData);
        }
        catch
        {
            Destroy(newBlock.gameObject);
        }
    }

    public void ChangeDragClass()
    {
        gameObject.AddComponent<UIDrag>();
        Destroy(gameObject.GetComponent<UIDragNew>());
    }

    void Start()
    {
        onDragCanvas = GameObject.FindGameObjectWithTag("onDragCanvas").transform;
    }
}
