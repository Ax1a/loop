using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IIndicatorController : MonoBehaviour
{
    public string interactableLayer;
    public GameObject indicator;

    void Start()
    {
        // Find all game objects in the scene
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        // Loop through the game objects and add the ones with the interactable layer to the list
        foreach (GameObject gameObject in allGameObjects)
        {
            if (gameObject.layer == LayerMask.NameToLayer(interactableLayer))
            {
                GameObject go = Instantiate(indicator);
                go.transform.parent = gameObject.transform;
                go.transform.localPosition = new Vector3(0f, .25f, 0f);
            }
        }
    }
}
