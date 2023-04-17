using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LessonInitPos : MonoBehaviour, IDropHandler
{
    public GameObject[] objects;
  
    GameObject parent;
    [HideInInspector] public LessonDragBlock drag;

    void Start()
    {
        parent = GameObject.FindGameObjectWithTag("parent");
    }

    public void ResetPositions()
    {
        // Reset the position of all objects to their initial positions
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.SetParent(parent.transform);
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {

            eventData.pointerDrag.GetComponent<LessonDragBlock>().ResetPos();
            Debug.Log("LessonInitPos: Reset Position");

        }
    }
}
