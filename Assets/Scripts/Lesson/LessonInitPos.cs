using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LessonInitPos : MonoBehaviour
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
            objects[i].GetComponent<LessonDragBlock>().parentToReturn = parent.transform;
        }
    }
}
