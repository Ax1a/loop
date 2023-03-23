using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IIndicatorController : MonoBehaviour
{
    public string interactableLayer;
    public GameObject indicator;

    void Start()
    {
        // int interactableLayerMask = 1 << LayerMask.NameToLayer(interactableLayer);
        // GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag(interactableLayerMask);

        // foreach (GameObject gameObject in interactableObjects)
        // {
        //     GameObject go = Instantiate(indicator);
        //     go.transform.parent = gameObject.transform;
        //     go.transform.localPosition = new Vector3(0f, .25f, 0f);
        // }
    }
}
