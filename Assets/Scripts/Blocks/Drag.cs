using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private Vector2 initPos;
    public int id;
    public GameObject original;
    // public GameObject clone;
    // public Vector3 offset;

    public Transform originalObject;

   public Transform parentAfterDrag;

    public GameObject parentObject;
    //public Image image;

    [SerializeField] private Canvas canvas;

    [SerializeField] public string blockType;


    private void Awake(){
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
 
    }

    void Start(){
        initPos = transform.position;
        
        
    }

    public void InstantiateNewObject(){
        GameObject newObject = Instantiate(originalObject.gameObject);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        parentAfterDrag = transform.parent;
       // transform.SetParent(transform.root);
        transform.SetParent(parentObject.transform);
        transform.SetAsLastSibling();
       // image.raycastTarget = false;
        // canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        
            
            
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
         
     //   rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        // transform.SetParent(parentAfterDrag);
        // image.raycastTarget = true;
        // canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      //  InstantiateNewObject();
        Debug.Log("OnPointerDown");
    }
    public void ResetPos(){
        transform.position = initPos;
    }
 

}
 