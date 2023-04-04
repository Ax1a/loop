using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class initialPosition : MonoBehaviour, IDropHandler
{
    public GameObject[] objects;
    public Vector3[] initialPositions;
    [HideInInspector] public Drag drag;
    GameObject parent;
    public static initialPosition Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Store the initial positions of all objects
        initialPositions = new Vector3[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            initialPositions[i] = objects[i].transform.position;
        }
        parent = GameObject.FindGameObjectWithTag("parent");

    }

    public void ResetPositions()
    {
        // Reset the position of all objects to their initial positions
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.SetParent(parent.transform);
            objects[i].transform.position = initialPositions[i];

        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {

            eventData.pointerDrag.GetComponent<Drag>().ResetPos();
            Debug.Log("Block reset position");
        }
    }
}
