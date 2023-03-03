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
   //  public Transform originalObject;
   public Transform parentAfterDrag;
    // public GameObject parentObject;
    //[SerializeField] private Canvas canvas;
    [SerializeField] public string blockType;


    private void Awake(){
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
 
    }

    void Start(){
        initPos = transform.position;  
    }

    // public void InstantiateNewObject(){
    //     GameObject newObject = Instantiate(originalObject.gameObject);
    // }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        // parentAfterDrag = transform.parent;
       // transform.SetParent(transform.root);
       // image.raycastTarget = false;
        // canvasGroup.alpha = .6f;
       // if(parentObject != null)
        // {
        // transform.SetParent(parentObject.transform);
        // transform.SetAsLastSibling();
        // }

        GameObject parent = GameObject.FindGameObjectWithTag("parent");
        transform.SetParent(parent.transform);
        transform.SetAsLastSibling();

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
        parentAfterDrag = transform.parent;
        canvasGroup.blocksRaycasts = true;
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      //  InstantiateNewObject();
        AudioManager.Instance.PlaySfx("Pop");
    }
    public void ResetPos(){
        transform.position = initPos;
    }
 

}
 